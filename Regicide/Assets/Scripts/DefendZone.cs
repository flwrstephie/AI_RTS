using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendZone : MonoBehaviour
{
    private DangerResourceManager dangerManager;
    private readonly List<GameObject> enemiesInZone = new List<GameObject>();

    private void Start()
    {
        dangerManager = FindObjectOfType<DangerResourceManager>();
        if (dangerManager == null)
        {
            Debug.LogError("[DefendZone] No DangerResourceManager found in scene!");
            enabled = false;
            return;
        }
        StartCoroutine(DangerLevelCheck());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemiesInZone.Contains(other.gameObject))
        {
            enemiesInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && enemiesInZone.Contains(other.gameObject))
        {
            enemiesInZone.Remove(other.gameObject);
        }
    }

    public void KillEnemy()
    {
        if (enemiesInZone.Count > 0)
        {
            GameObject target = enemiesInZone[0];
            enemiesInZone.RemoveAt(0);

            EnemyAI enemyAI = target.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Debug.Log($"[DefendZone] Killing enemy: {enemyAI.name}");
                StartCoroutine(PlayDeathAndDestroy(enemyAI));
            }
        }
        else
        {
            Debug.Log("[DefendZone] No enemies in zone.");
        }
    }

    private IEnumerator PlayDeathAndDestroy(EnemyAI enemyAI)
    {
        Animator animator = enemyAI.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Die"); // make sure this trigger exists in the Animator
        }

        // Disable movement
        enemyAI.enabled = false;

        yield return new WaitForSeconds(1.5f); // wait for death animation (adjust as needed)
        Destroy(enemyAI.gameObject);
    }



    private IEnumerator DangerLevelCheck()
    {
        while (true)
        {
            if (enemiesInZone.Count > 0)
            {
                yield return new WaitForSeconds(3f);
                int dangerDrop = Random.Range(1, 4);
                dangerManager.DangerLevel = Mathf.Max(0, dangerManager.DangerLevel - dangerDrop);
                Debug.Log($"[DefendZone] DangerLevel -{dangerDrop} (Enemies in zone)");
            }
            else
            {
                yield return new WaitForSeconds(1f);
                dangerManager.DangerLevel += 1;
                Debug.Log("[DefendZone] DangerLevel +1 (Zone is clear)");
            }
        }
    }
}
