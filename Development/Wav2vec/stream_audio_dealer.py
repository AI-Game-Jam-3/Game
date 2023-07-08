
import pyaudio
import wave
import torch
import soundfile as sf
from transformers import AutoProcessor, Wav2Vec2ForCTC

# 缓存中存放帧数
CHUNK = 8000
# 量化位数为16位
RESPEAKER_WIDTH = 2
# 声道数,1声道为ASR处理语音，2-5声道为未经处理的麦克风单声道语音，6声道为合并语音（6声道未显示过）
RESPEAKER_CHANNELS = 1
# 对于语音信号的频率范围通常更窄（低于8000Hz）,因此通常使用16000Hz的采样率；
RESPEAKER_RATE = 16000
# 采样时间
RECORD_SECONDS = 5
# 输出文件名称
WAVE_OUTPUT_FILENAME = "output1.wav"
# input device index
INPUT_RESPEAKER_INDEX = 1
OUTPUT_RESPEAKER_INDEX = 5

processor = AutoProcessor.from_pretrained("facebook/wav2vec2-base-960h")
model = Wav2Vec2ForCTC.from_pretrained("facebook/wav2vec2-base-960h")
# 初始化pyaudio,sets up the portaudio system
p = pyaudio.PyAudio()
c = 0
frames = []
flag = True


if __name__ == "__main__":
    p = pyaudio.PyAudio()
    c = 0

    frames = []



    input_stream = p.open(
        # 采样率
        rate=RESPEAKER_RATE,
        # 量化位数
        format=p.get_format_from_width(RESPEAKER_WIDTH),
        # 语音通道数
        channels=RESPEAKER_CHANNELS,
        # 开启record
        input=True,
        # 具体输入设备
        # output=True,
        # output_device_index=OUTPUT_RESPEAKER_INDEX,
        # stream_callback=callback,
        frames_per_buffer=CHUNK,
    )
    # start the stream (4)

    # audio file is decoded on the fly


    while flag:
        input_data = input_stream.read(CHUNK)
        with wave.open("tmp.wav", "wb") as wf:
            wf.setnchannels(RESPEAKER_CHANNELS)
            wf.setsampwidth(pyaudio.get_sample_size(RESPEAKER_WIDTH))
            wf.setframerate(RESPEAKER_RATE)
            wf.writeframes(input_data)
        input_audio = sf.read("tmp.wav")[0]
        inputs = processor(input_audio, sampling_rate=RESPEAKER_RATE, return_tensors="pt")
        with torch.no_grad():
            logits = model(**inputs).logits
        predicted_ids = torch.argmax(logits, dim=-1)

        # transcribe speech
        transcription = processor.batch_decode(predicted_ids)
        print(transcription[0])
        # output_stream.write(input_data)

    input_stream.stop_stream()
    input_stream.close()

    # close PyAudio (7)
    p.terminate()
