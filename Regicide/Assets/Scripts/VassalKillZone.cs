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
            Debug.Log($"[VassalKillZone] Killing vassal: {target.name}");
            Destroy(target);
            vassalsInZone.RemoveAt(0);
            return true;
        }

        Debug.Log("[VassalKillZone] No vassals in zone to kill.");
        return false;
    }
}
