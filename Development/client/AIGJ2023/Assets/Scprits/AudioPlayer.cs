using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public List<string> audioName;
    public List<AudioClip> audioClip;

    private Dictionary<string, AudioSource> audioSource;

    private static AudioPlayer instance;
    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioPlayer>();
            }
            return instance;
        }
    }

    void Start()
    {
        audioSource = new Dictionary<string, AudioSource>();
        for(int i = 0; i < audioName.Count; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = audioClip[i];
            source.playOnAwake = false;
            source.loop = false;
            source.Stop();
            audioSource.Add(audioName[i], source);
        }
    }

    public void PlayClip(string clip, float len = 0, float volumn = 1, bool isLoop = false)
    {
        var source = audioSource[clip];
        if (source.isPlaying) source.Stop();
        source.volume = volumn;
        source.loop = isLoop;
        source.Play();
    }

    public void StopClip(string clip)
    {
        audioSource[clip].Stop();
    }

}
