using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnRate = 2f;        // Meteors per second
    public float xRange = 8f;           // Horizontal spawn range
    public float ySpawn = 10f;          // Height to spawn

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / spawnRate)
        {
            SpawnMeteor();
            timer = 0f;
        }
    }

   private void SpawnMeteor()
{
    Vector2 spawnPos = new Vector2(Random.Range(-xRange, xRange), ySpawn);
    GameObject meteorInstance = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

    // Random scale
    //float randomScale = Random.Range(0.5f, 1.5f);
    //meteorInstance.transform.localScale = Vector3.one * randomScale;

    // Assign a straight downward direction with a random horizontal angle
    Meteor meteorScript = meteorInstance.GetComponent<Meteor>();
    if (meteorScript != null)
    {
        float angle = Random.Range(-30f, 30f); // degrees left/right
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.down; 
        meteorScript.SetDirection(dir);
    }
}


}
