using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    private Coroutine musicFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolumes();

        if (menuMusic != null)
            PlayMusic(menuMusic);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(
            clip,
            Mathf.Clamp01(sfxVolume)
        );
    }

    public void PlaySound(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;

        switch (name.ToLowerInvariant())
        {
            case "click":
                PlaySound(clickSound);
                break;

            case "correct":
                PlaySound(correctSound);
                break;

            case "incorrect":
                PlaySound(incorrectSound);
                break;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeTime = 1f)
    {
        if (clip == null || musicSource == null)
            return;

        StopMusicFade();

        musicFadeCoroutine = StartCoroutine(
            FadeInMusic(clip, Mathf.Max(0f, fadeTime))
        );
    }

    public void StopMusic(float fadeTime = 1f)
    {
        if (musicSource == null)
            return;

        StopMusicFade();

        musicFadeCoroutine = StartCoroutine(
            FadeOutMusic(Mathf.Max(0f, fadeTime))
        );
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    private void StopMusicFade()
    {
        if (musicFadeCoroutine == null)
            return;

        StopCoroutine(musicFadeCoroutine);
        musicFadeCoroutine = null;
    }

    private IEnumerator FadeInMusic(AudioClip clip, float duration)
    {
        if (musicSource == null)
            yield break;

        if (musicSource.isPlaying)
            yield return FadeOutMusic(duration / 2f);

        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();

        if (duration <= 0f)
        {
            musicSource.volume = musicVolume;
            musicFadeCoroutine = null;
            yield break;
        }

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(
                0f,
                musicVolume,
                Mathf.Clamp01(timer / duration)
            );

            yield return null;
        }

        musicSource.volume = musicVolume;
        musicFadeCoroutine = null;
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        if (musicSource == null)
            yield break;

        float startVolume = musicSource.volume;

        if (duration <= 0f)
        {
            musicSource.Stop();
            musicSource.volume = musicVolume;
            yield break;
        }

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(
                startVolume,
                0f,
                Mathf.Clamp01(timer / duration)
            );

            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = musicVolume;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}