import os
import time
import sounddevice as sd
import numpy as np
import torch
from transformers import Wav2Vec2ForCTC, Wav2Vec2Processor
from scipy.io.wavfile import write
import librosa

def word_check(sentence):
    word_table = {
        'break': 'break',
        'brak': 'break',
        'brigg': 'break',
        'brik': 'break',
        'brick': 'break',
        'braker': 'break',
        'brig': 'break',
        'bridge': 'break',
        'breek': 'break',
        'breech': 'break',
        'brait': 'break',
        'bric': 'break',
        'great': 'break',
        'bridg': 'break',
        'breag': 'break',
        'breeg': 'break',
        'boit': 'break',
        'frac': 'break',
        'greek': 'break',

        'fire': 'fire',
        'fair': 'fire',
        'fry': 'fire',
        'fly': 'fire',
    }

    words = sentence.lower().split()
    result = set()  # 使用集合来存储结果

    for word in words:
        if word in word_table:
            result.add(word_table[word])  # 添加到集合中

    return list(result)  # 将集合转换为列表并返回

# 初始化处理器和模型
processor = Wav2Vec2Processor.from_pretrained("facebook/wav2vec2-base-960h")
model = Wav2Vec2ForCTC.from_pretrained("facebook/wav2vec2-base-960h")

# 定义一些参数
fs = 16000  # 采样率
filename = 'recorded.wav'  # 保存音频的文件名
threshold_high = 0.5  # 设置一个音量阈值，当音量超过此阈值时，开始录音
threshold_low = 0.5  # 设置一个音量阈值，当音量低于此阈值时，结束录音
block_duration = 0.01  # 单位为秒

recording = False  # 是否正在录音
buffer = []  # 存储录音数据
low_volumes = 0  # 记录连续低音量的个数

# 定义一个回调函数来处理获取的音频数据
def callback(indata, frames, time, status):
    global recording, buffer, low_volumes
    volume_norm = np.linalg.norm(indata)*10
    #print('Volume:', volume_norm)

    # 检查是否正在录音
    if recording:
        # 检查音量是否低于阈值，如果是，则增加低音量计数
        if volume_norm < threshold_low:
            low_volumes += 1
            # 如果低音量计数超过设定的次数，则结束录音
            if low_volumes >= int(1.0 / block_duration):
                print('Stop recording')
                recording = False
                low_volumes = 0
                # 把录音数据写入文件
                write(filename, fs, np.concatenate(buffer, axis=0))
                buffer.clear()
        else:
            low_volumes = 0  # 重置低音量计数
            buffer.append(indata.copy())
    else:
        # 检查音量是否超过阈值，如果是，则开始录音
        if volume_norm > threshold_high:
            print('Start recording')
            recording = True
            buffer.append(indata.copy())

with sd.InputStream(callback=callback, channels=1, samplerate=fs, blocksize=int(fs * block_duration)):
    while True:
        # 检查是否有录音文件
        if os.path.exists(filename):
            # 从文件中加载音频数据
            audio, _ = librosa.load(filename, sr=fs)

            # 对音频数据进行处理
            inputs = processor(audio, sampling_rate=fs, return_tensors="pt", padding=True)

            with torch.no_grad():
                logits = model(inputs.input_values).logits

            predicted_ids = torch.argmax(logits, dim=-1)
            transcription = processor.decode(predicted_ids[0])
            print('Transcription: ', transcription)
            print(word_check(transcription))
            # 删除文件以准备下一次录音
            os.remove(filename)

        time.sleep(1)