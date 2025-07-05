using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class VassalSoundZone : MonoBehaviour
{
    public string zoneTag; // Must match one of: Kitchen, Factory, Cave, CastleWall, Cannon

    [Header("Sound Loops")]
    public AudioClip kitchenLoop;
    public AudioClip factoryLoop;
    public AudioClip caveLoop;
    public AudioClip castleWallLoop;
    [Range(0f, 1f)] public float loopVolume = 0.5f;
    public float fadeOutDuration = 1.5f;

    private Dictionary<string, AudioClip> zoneSounds;
    private HashSet<Rigidbody> activeVassals = new HashSet<Rigidbody>();
    private AudioSource zoneAudioSource;
    private Coroutine fadeOutCoroutine;

    private void Awake()
    {
        if (!GetComponent<Collider>().isTrigger)
        {
            Debug.Log("[Zone] Using non-trigger collider. Good!");
        }

        zoneAudioSource = gameObject.AddComponent<AudioSource>();
        zoneAudioSource.loop = true;
        zoneAudioSource.playOnAwake = false;
        zoneAudioSource.volume = loopVolume;

        zoneSounds = new Dictionary<string, AudioClip>
        {
            { "Kitchen", kitchenLoop },
            { "Factory", factoryLoop },
            { "Cave", caveLoop },
            { "CastleWall", castleWallLoop }
        };

        if (!zoneSounds.ContainsKey(zoneTag))
        {
            Debug.LogWarning($"Zone '{gameObject.name}' has unrecognized tag: {zoneTag}");
        }
        else
        {
            zoneAudioSource.clip = zoneSounds[zoneTag];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsValidVassal(collision.collider, out Rigidbody rb))
        {
            if (activeVassals.Add(rb) && activeVassals.Count == 1)
            {
                if (fadeOutCoroutine != null)
                    StopCoroutine(fadeOutCoroutine);

                zoneAudioSource.volume = loopVolume;
                zoneAudioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsValidVassal(collision.collider, out Rigidbody rb))
        {
            if (activeVassals.Remove(rb) && activeVassals.Count == 0)
            {
                if (fadeOutCoroutine != null)
                    StopCoroutine(fadeOutCoroutine);

                fadeOutCoroutine = StartCoroutine(FadeOutAndStop());
            }
        }
    }

    private bool IsValidVassal(Collider col, out Rigidbody rb)
    {
        rb = col.attachedRigidbody;
        return rb != null && (
            col.CompareTag("Kitchen") ||
            col.CompareTag("Factory") ||
            col.CompareTag("Cave") ||
            col.CompareTag("CastleWall") ||
            col.CompareTag("Cannon")
        );
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = zoneAudioSource.volume;

        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            zoneAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        zoneAudioSource.volume = 0f;
        zoneAudioSource.Stop();
        zoneAudioSource.volume = loopVolume;
        fadeOutCoroutine = null;
    }

    private void FadeOutImmediately()
    {
        if (zoneAudioSource != null && zoneAudioSource.isPlaying)
        {
            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine);

            zoneAudioSource.volume = 0f;
            zoneAudioSource.Stop();
        }
    }
}
