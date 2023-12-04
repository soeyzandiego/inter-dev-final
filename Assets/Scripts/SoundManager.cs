using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static float defaultVol = 0.9f;

    static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }

    public static void PlaySound(AudioClip clip)
    {
        audioSource.volume = defaultVol;
        audioSource.PlayOneShot(clip);
    }
}
