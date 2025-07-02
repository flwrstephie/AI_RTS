using UnityEngine;
using UnityEngine.SceneManagement;

public class VassalKillScript : MonoBehaviour
{
    private DangerResourceManager drm;

    private float dangerZeroTimer = 0f;
    private float hungerZeroTimer = 0f;

    private const float DangerZeroThreshold = 15f;
    private const float HungerZeroThreshold = 30f;
    private VassalKillZone killZone;

    private void Start()
    {
        drm = FindObjectOfType<DangerResourceManager>();
        killZone = FindObjectOfType<VassalKillZone>();

        if (drm == null)
            Debug.LogError("[VassalKillScript] DangerResourceManager not found in scene.");
        if (killZone == null)
            Debug.LogError("[VassalKillScript] VassalKillZone not found in scene.");
    }

    private void Update()
    {
        if (drm == null) return;

        if (drm.VassalNumber <= 0){
            SceneManager.LoadScene("LoseScene");
        }
        // Danger Level Tracking
        if (drm.DangerLevel <= 15)
        {
            dangerZeroTimer += Time.deltaTime;
            if (dangerZeroTimer >= DangerZeroThreshold)
            {
                KillVassal("Danger");
                dangerZeroTimer = 0f; // Reset after a kill
            }
        }
        else
        {
            dangerZeroTimer = 0f;
        }

        // Hunger Level Tracking
        if (drm.HungerLevel <= 15)
        {
            hungerZeroTimer += Time.deltaTime;
            if (hungerZeroTimer >= HungerZeroThreshold)
            {
                KillVassal("Hunger");
                hungerZeroTimer = 0f; // Reset after a kill
            }
        }
        else
        {
            hungerZeroTimer = 0f;
        }
    }

    private void KillVassal(string cause)
    {
        if (drm.VassalNumber > 0)
        {
            bool killed = killZone != null && killZone.KillVassalInZone();
            if (killed)
            {
                drm.VassalNumber -= 1;
                Debug.Log($"[VassalKillScript] Vassal died due to {cause} starvation.");
            }
            else
            {
                Debug.Log($"[VassalKillScript] Couldn't kill a vassal GameObject, but count decreased.");
                drm.VassalNumber -= 1;
            }
        }
        else
        {
            Debug.Log("[VassalKillScript] No vassals left to kill.");
        }
    }
}
