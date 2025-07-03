using UnityEngine;

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

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
}
