using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Meteor Settings")]
    public GameObject meteorPrefab;
    public float xRange = 8f;
    public float ySpawn = 10f;

    [Header("Burst Settings")]
    public int meteorsPerBurst = 5;
    public float burstInterval = 2f;

    [Header("Difficulty Settings")]
    public float difficultyLevel = 1f;   // <-- make float so we can multiply by 1.1
    public float angleRange = 60f;

    private bool spawning = false;
    private float spawnTimer = 0f;
    private float spawnInterval = 120f;

    void Update()
    {
        // Count up with deltaTime
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            TriggerSpawn();
            difficultyLevel *= 1.1f;
            spawnTimer = 0f;
            Debug.Log($"Difficulty increased to {difficultyLevel:F2}");
        }
    }

    public void TriggerSpawn()
    {
        if (!spawning)
            StartCoroutine(SpawnBurst());
    }

    private System.Collections.IEnumerator SpawnBurst()
    {
        spawning = true;

        int totalMeteors = Mathf.RoundToInt(meteorsPerBurst * difficultyLevel);
        for (int i = 0; i < totalMeteors; i++)
        {
            SpawnMeteor();
            yield return new WaitForSeconds(burstInterval);
        }

        spawning = false;
    }

    private void SpawnMeteor()
    {
        Vector2 spawnPos = new Vector2(
            transform.position.x + Random.Range(-xRange, xRange),
            transform.position.y + ySpawn
        );

        GameObject meteorInstance = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        meteorInstance.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        Meteor meteorScript = meteorInstance.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            float angle = Random.Range(-angleRange, angleRange);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.down;
            meteorScript.SetDirection(dir);

            float spinSpeed = Random.Range(-180f, 180f);
            meteorScript.SetSpin(spinSpeed);

            meteorScript.hp = Mathf.CeilToInt(meteorScript.hp * difficultyLevel);
            meteorScript.power = Mathf.CeilToInt(meteorScript.power * difficultyLevel);
        }
    }
}
