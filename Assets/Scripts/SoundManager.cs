using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource voiceSource;
    public AudioSource clapSource;
    public AudioSource clickSource;

    [Header("Voice Clip")]
    public AudioClip introClip;

    [Header("Clap Settings")]
    public float clapDuration = 9f;

    private Coroutine clapCoroutine;

    void Start()
    {
        // 🔊 Play intro once
        PlayIntro();
    }

    // 🔊 Intro Voice
    public void PlayIntro()
    {
        if (voiceSource == null || introClip == null) return;

        voiceSource.clip = introClip;
        voiceSource.Play();
    }

    // 👏 Clap (controlled for duration)
    public void PlayClap()
    {
        if (clapSource == null) return;

        if (clapCoroutine != null)
            StopCoroutine(clapCoroutine);

        clapCoroutine = StartCoroutine(PlayClapRoutine());
    }

    IEnumerator PlayClapRoutine()
    {
        clapSource.loop = true;
        clapSource.Play();

        yield return new WaitForSeconds(clapDuration);

        clapSource.Stop();
    }

    // 🔘 Click
    public void PlayClick()
    {
        if (clickSource != null)
            clickSource.Play();
    }
}