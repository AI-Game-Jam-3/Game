using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public List<string> audioName;
    public List<AudioClip> audioClip;

    private Dictionary<string, AudioSource> playingAudio;

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
        playingAudio = new Dictionary<string, AudioSource>();
    }

    IEnumerator WaitAndDestroyAudioSource(string name, AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        playingAudio.Remove(name);
        GameObject.Destroy(source);
    }

    public void PlayClip(string clip, float len = 0, float volumn = 1, bool isLoop = false)
    {
        for(int i = 0; i < audioName.Count; i++)
        {
            if (audioName[i] == clip)
            {
                if(playingAudio.ContainsKey(clip))
                    GameObject.Destroy(playingAudio[clip]);
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioClip[i];
                audioSource.volume = volumn;
                audioSource.loop = isLoop;
                audioSource.Play();
                if (!isLoop)
                {
                    if (len == 0)
                        StartCoroutine(WaitAndDestroyAudioSource(clip, audioSource, audioClip[i].length));
                    else
                        StartCoroutine(WaitAndDestroyAudioSource(clip, audioSource, len));
                }
            }
        }
    }

}
