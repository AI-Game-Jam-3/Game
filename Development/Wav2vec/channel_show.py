import pyaudio


def get_audio_devices():
    p = pyaudio.PyAudio()
    devices = []
    print(p.get_device_count())
    for i in range(p.get_device_count()):
        print(i)
        print(p.get_device_info_by_index(i).get('name'))
        devices.append(p.get_device_info_by_index(i))
    return devices


def get_audio_input_devices():
    devices = []
    for item in get_audio_devices():
        if item.get('maxInputChannels') > 0:
            devices.append(item)
    return devices


print(get_audio_input_devices())
