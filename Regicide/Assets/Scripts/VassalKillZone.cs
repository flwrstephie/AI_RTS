using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VassalKillZone : MonoBehaviour
{
    private readonly List<GameObject> vassalsInZone = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vassal") && !vassalsInZone.Contains(other.gameObject))
        {
            vassalsInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vassal") && vassalsInZone.Contains(other.gameObject))
        {
            vassalsInZone.Remove(other.gameObject);
        }
    }

    public bool KillVassalInZone()
    {
        if (vassalsInZone.Count > 0)
        {
            GameObject target = vassalsInZone[0];
            vassalsInZone.RemoveAt(0);

            Animator animator = target.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                target.GetComponent<Collider>().enabled = false;

                MonoBehaviour mono = target.GetComponent<MonoBehaviour>(); // to run coroutine
                if (mono != null)
                    mono.StartCoroutine(PlayDeathAndDestroy(animator, target));

                return true;
            }

            // fallback if no animator
            Debug.Log($"[VassalKillZone] Instantly destroying vassal: {target.name}");
            Destroy(target);
            return true;
        }

        Debug.Log("[VassalKillZone] No vassals in zone to kill.");
        return false;
    }
    private IEnumerator PlayDeathAndDestroy(Animator animator, GameObject target)
    {
        animator.SetTrigger("Die"); // Trigger death animation
        yield return new WaitForSeconds(1.5f); // Wait for death anim (adjust time if needed)
        Destroy(target);
    }

}
