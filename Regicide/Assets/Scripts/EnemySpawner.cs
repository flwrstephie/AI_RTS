using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Y Offset")]
    public float offsetY = 0f;

    [Header("X-Range (Relative to this object)")]
    public float minXOffset = -75f;
    public float maxXOffset = 75f;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[EnemySpawner] Enemy Prefab not assigned!");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(12f, 18f);
            yield return new WaitForSeconds(delay);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        AudioManager.Instance.PlayEnemySpawn();
        float randomX = Random.Range(minXOffset, maxXOffset);
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y + offsetY,
            transform.position.z
        );

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemy.transform.SetParent(null);

        // Directly assign moveSpeed
        spawnedEnemy.GetComponent<EnemyAI>().moveSpeed = Random.Range(-12f, -6f);

        Debug.Log($"[EnemySpawner] Spawned {enemyPrefab.name} at {spawnPosition}");
    }
}
