using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public List<string> audioName;
    public List<AudioClip> audioClip;

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

    IEnumerator WaitAndDestroyAudioSource(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(source);
    }

    public void PlayClip(string clip, float len = 0, float volumn = 1, bool isLoop = false)
    {
        for(int i = 0; i < audioName.Count; i++)
        {
            if (audioName[i] == clip)
                PlayClip(audioClip[i], len, volumn, isLoop);
        }
    }

    public void PlayClip(AudioClip clip, float len = 0, float volumn = 1, bool isLoop = false)
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volumn;
        audioSource.loop = isLoop;
        audioSource.Play();
        if (!isLoop)
        {
            if (len == 0)
                StartCoroutine(WaitAndDestroyAudioSource(audioSource, clip.length));
            else
                StartCoroutine(WaitAndDestroyAudioSource(audioSource, len));
        }
    }

}
