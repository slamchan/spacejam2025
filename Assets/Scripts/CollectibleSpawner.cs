using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] collectiblePrefabs; // assign in Inspector
    public float spawnInterval = 5f;        // seconds between spawns
    public int maxCollectibles = 20;        // limit in the world at once

    [Header("Spawn Area")]
    public Vector2 areaSize = new Vector2(20f, 10f); // width/height of area
    public Transform centerPoint;                    // optional, center of area

    private float timer;

    void Start()
    {
        if (centerPoint == null)
            centerPoint = transform; // fallback to spawner’s transform
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }
    }

    void TrySpawn()
    {
        // Limit total collectibles
        if (GameObject.FindGameObjectsWithTag("Collectible").Length >= maxCollectibles)
            return;

        // Pick random prefab
        GameObject prefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];

        // Pick random position within area
        Vector2 spawnPos = (Vector2)centerPoint.position + new Vector2(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );

        // Spawn
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the spawn area in the editor
        Gizmos.color = Color.green;
        Vector3 center = centerPoint != null ? centerPoint.position : transform.position;
        Gizmos.DrawWireCube(center, new Vector3(areaSize.x, areaSize.y, 0));
    }
}
