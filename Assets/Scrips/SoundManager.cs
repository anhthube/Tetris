using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    // Start is called before the first frame update
    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Lặp lại nhạc nền
        audioSource.Play();
    }
    public void PlaySoundForDuration(AudioClip clip, float duration)
    {
        StartCoroutine(PlayAndStopAfterDuration(clip, duration));
    }

    private IEnumerator PlayAndStopAfterDuration(AudioClip clip, float duration)
    {
        
        audioSource.PlayOneShot(clip); 
        yield return new WaitForSeconds(duration); 
        
    }
}
