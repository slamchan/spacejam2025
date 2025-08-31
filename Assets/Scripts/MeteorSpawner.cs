using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Meteor Settings")]
    public GameObject meteorPrefab;
    public float xRange = 8f;        // Horizontal spawn range
    public float ySpawn = 10f;       // Height to spawn

    [Header("Burst Settings")]
    public int meteorsPerBurst = 20;  // How many meteors per trigger
    public float burstInterval = 0.2f; // Delay between meteors in a burst

    [Header("Difficulty Settings")]
    public int difficultyLevel = 1;  // Difficulty multiplier
    public float angleRange = 60f;   // Max deviation from straight down

    private bool spawning = false;

    /// <summary>
    /// Call this from outside (e.g. button, timer, building, etc.)
    /// </summary>
    public void TriggerSpawn()
    {
        if (!spawning)
            StartCoroutine(SpawnBurst());
    }

    private System.Collections.IEnumerator SpawnBurst()
    {
        spawning = true;

        int totalMeteors = meteorsPerBurst * difficultyLevel;
        for (int i = 0; i < totalMeteors; i++)
        {
            SpawnMeteor();
            yield return new WaitForSeconds(burstInterval);
        }

        spawning = false;
    }

    private void SpawnMeteor()
    {
        // Spawn directly above the spawner/base
        Vector2 spawnPos = new Vector2(
            transform.position.x + Random.Range(-xRange, xRange),  // optional horizontal spread
            transform.position.y + ySpawn                            // height above the spawner
        );

        GameObject meteorInstance = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        meteorInstance.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        Meteor meteorScript = meteorInstance.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            float angle = Random.Range(-angleRange, angleRange);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.down;
            meteorScript.SetDirection(dir);

            // Add random spin
            float spinSpeed = Random.Range(-180f, 180f); // degrees/sec
            meteorScript.SetSpin(spinSpeed);

            // Scale with difficulty
            meteorScript.hp = Mathf.CeilToInt(meteorScript.hp * difficultyLevel);
            meteorScript.power = Mathf.CeilToInt(meteorScript.power * difficultyLevel);
        }
    }

}
