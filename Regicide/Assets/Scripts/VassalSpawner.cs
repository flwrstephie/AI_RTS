using UnityEngine;

public class VassalSpawner : MonoBehaviour
{
    [Header("Vassal Prefab")]
    public GameObject vassalPrefab;

    [Header("Spawn Area Container (Must Have BoxCollider)")]
    public GameObject spawnArea;

    [Header("Number of Vassals to Spawn")]
    public int vassalCount = 30;

    private BoxCollider spawnCollider;

    private void Start()
    {
        if (vassalPrefab == null || spawnArea == null)
        {
            Debug.LogError("[VassalSpawner] Missing references.");
            return;
        }

        spawnCollider = spawnArea.GetComponent<BoxCollider>();
        if (spawnCollider == null)
        {
            Debug.LogError("[VassalSpawner] Spawn area needs a BoxCollider component.");
            return;
        }

        SpawnVassals();
    }

    private void SpawnVassals()
    {
        for (int i = 0; i < vassalCount; i++)
        {
            Vector3 randomPosition = GetRandomPointInBounds(spawnCollider.bounds);
            Instantiate(vassalPrefab, randomPosition, Quaternion.identity);
        }

        Debug.Log($"[VassalSpawner] Spawned {vassalCount} vassals.");
    }

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
