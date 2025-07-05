using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SoundEffect
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip mainMenuBGM;
    public AudioClip gameBGM;
    public AudioClip gameOverBGM;

    [Header("Looping Task Clips")]
    public AudioClip exploreLoopClip;
    [Range(0f, 1f)] public float exploreLoopVolume = 1f;

    public AudioClip kitchenLoopClip;
    [Range(0f, 1f)] public float kitchenLoopVolume = 1f;

    public AudioClip factoryLoopClip;
    [Range(0f, 1f)] public float factoryLoopVolume = 1f;

    public AudioClip defendLoopClip;
    [Range(0f, 1f)] public float defendLoopVolume = 1f;

    [Header("SFX")]
    public SoundEffect sfxButton;
    public SoundEffect sfxVassalSelect1;
    public SoundEffect sfxVassalSelect2;
    public SoundEffect sfxVassalYes1;
    public SoundEffect sfxVassalYes2;
    public SoundEffect sfxVassalDeath1;
    public SoundEffect sfxVassalDeath2;
    public SoundEffect sfxESpawn1;
    public SoundEffect sfxESpawn2;
    public SoundEffect sfxEAttack1;
    public SoundEffect sfxEAttack2;
    public SoundEffect sfxEDeath1;
    public SoundEffect sfxEDeath2;
    public SoundEffect QueenDialog1;
    public SoundEffect QueenDialog2;
    public SoundEffect QueenDialog3;
    private SoundEffect[] QueenDialogs;

    private AudioSource exploreSource;
    private AudioSource kitchenSource;
    private AudioSource factorySource;
    private AudioSource defendSource;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        exploreSource = gameObject.AddComponent<AudioSource>();
        kitchenSource = gameObject.AddComponent<AudioSource>();
        factorySource = gameObject.AddComponent<AudioSource>();
        defendSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { exploreSource, kitchenSource, factorySource, defendSource })
        {
            src.loop = true;
            src.playOnAwake = false;
        }

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        QueenDialogs = new[] { QueenDialog1, QueenDialog2, QueenDialog3 };
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
            bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
    }

    public void PlaySFX(SoundEffect sfx)
    {
        if (sfxSource != null && sfx.clip != null)
            sfxSource.PlayOneShot(sfx.clip, sfx.volume);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource != null && clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
    }

    public void PlayButtonClick()
    {
        PlaySFX(sfxButton);
    }

    public void PlayVassalSelect()
    {
        PlaySFX(Random.value < 0.5f ? sfxVassalSelect1 : sfxVassalSelect2);
    }

    public void PlayVassalYes()
    {
        PlaySFX(Random.value < 0.5f ? sfxVassalYes1 : sfxVassalYes2);
    }

    public void PlayVassalDeath()
    {
        PlaySFX(Random.value < 0.5f ? sfxVassalDeath1 : sfxVassalDeath2);
    }

    public void PlayEnemySpawn()
    {
        PlaySFX(Random.value < 0.5f ? sfxESpawn1 : sfxESpawn2);
    }

    public void PlayEnemyAttack()
    {
        PlaySFX(Random.value < 0.5f ? sfxEAttack1 : sfxEAttack2);
    }

    public void PlayEnemyDeath()
    {
        PlaySFX(Random.value < 0.5f ? sfxEDeath1 : sfxEDeath2);
    }

    public void PlayQueenDialog()
    {
        if (QueenDialogs.Length > 0)
        {
            int index = Random.Range(0, QueenDialogs.Length);
            PlaySFX(QueenDialogs[index]);
        }
    }

    public void StopAllLoopingTasks()
    {
        StopLoopWithFade(exploreSource);
        StopLoopWithFade(kitchenSource);
        StopLoopWithFade(factorySource);
        StopLoopWithFade(defendSource);
    }

    public void PlayTaskLoop(string taskName)
    {
        switch (taskName)
        {
            case "Explore":
                if (exploreSource.clip == exploreLoopClip && exploreSource.isPlaying)
                    return;

                exploreSource.clip = exploreLoopClip;
                exploreSource.volume = exploreLoopVolume;
                exploreSource.Play();
                break;

            case "Kitchen":
                if (kitchenSource.clip == kitchenLoopClip && kitchenSource.isPlaying)
                    return;

                kitchenSource.clip = kitchenLoopClip;
                kitchenSource.volume = kitchenLoopVolume;
                kitchenSource.Play();
                break;

            case "Factory":
                if (factorySource.clip == factoryLoopClip && factorySource.isPlaying)
                    return;

                factorySource.clip = factoryLoopClip;
                factorySource.volume = factoryLoopVolume;
                factorySource.Play();
                break;

            case "Defend":
                if (defendSource.clip == defendLoopClip && defendSource.isPlaying)
                    return;

                defendSource.clip = defendLoopClip;
                defendSource.volume = defendLoopVolume;
                defendSource.Play();
                break;
        }
    }

    public void StopTaskLoop(string taskName)
    {
        switch (taskName)
        {
            case "Explore": StopLoopWithFade(exploreSource); break;
            case "Kitchen": StopLoopWithFade(kitchenSource); break;
            case "Factory": StopLoopWithFade(factorySource); break;
            case "Defend": StopLoopWithFade(defendSource); break;
        }
    }

    private void StopLoopWithFade(AudioSource source)
    {
        if (source.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(source, 0.5f));
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset for future use
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenuScene":
                PlayBGM(mainMenuBGM);
                break;
            case "GameScene":
                PlayBGM(gameBGM);
                break;
            case "WinScene":
            case "LoseScene":
                PlayBGM(gameOverBGM);
                break;
        }
    }
}
