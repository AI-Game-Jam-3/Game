import os
import time
import sounddevice as sd
import numpy as np
import torch
from transformers import Wav2Vec2ForCTC, Wav2Vec2Processor
from scipy.io.wavfile import write
import librosa

# 定义一些参数
duration = 10  # 监听音频的持续时间（秒）
fs = 16000  # 采样率
filename = 'recorded.wav'  # 保存音频的文件名
threshold = 0.1  # 设置一个音量阈值，当音量超过此阈值时，开始录音

# 初始化处理器和模型
processor = Wav2Vec2Processor.from_pretrained("facebook/wav2vec2-base-960h")
model = Wav2Vec2ForCTC.from_pretrained("facebook/wav2vec2-base-960h")

# 定义一个回调函数来处理获取的音频数据
def callback(indata, frames, time, status):
    print("callback")
    volume_norm = np.linalg.norm(indata)*10
    if volume_norm > threshold:
        print('Recording audio...')
        write(filename, fs, indata)

# 使用麦克风录制实时音频
while True:  # 主循环开始
    with sd.InputStream(callback=callback, channels=1, samplerate=fs, blocksize=int(fs * duration)):
        print('Listening...')
        sd.sleep(duration * 1000)

    # 等待文件被创建
    while not os.path.exists(filename):
        print('Waiting for file to be created...')
        time.sleep(1)

    # 从文件中加载音频数据
    audio, _ = librosa.load(filename, sr=fs)

    # 对音频数据进行处理
    inputs = processor(audio, sampling_rate=fs, return_tensors="pt", padding=True)

    with torch.no_grad():
        logits = model(inputs.input_values).logits

    predicted_ids = torch.argmax(logits, dim=-1)
    transcription = processor.decode(predicted_ids[0])
    print('Transcription: ', transcription)