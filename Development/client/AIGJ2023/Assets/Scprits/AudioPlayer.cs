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

    IEnumerator WaitAndDestroyAudioSource(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        GameObject.Destroy(source);
    }

    public void PlayClip(string clip, float volumn = 1, bool isLoop = false)
    {
        for(int i = 0; i < audioClip.Count; i++)
        {
            if (audioClip[i].name == clip)
                PlayClip(audioClip[i], volumn, isLoop);
        }
    }

    public void PlayClip(AudioClip clip, float volumn = 1, bool isLoop = false)
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volumn;
        audioSource.loop = isLoop;
        audioSource.Play();
        if(!isLoop) StartCoroutine(WaitAndDestroyAudioSource(audioSource));
    }

}
