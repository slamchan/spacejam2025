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

        // Instantiate the meteor
        GameObject meteorInstance = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // Randomize scale (applied to the instance, not the prefab)
        //float randomScale = Random.Range(0.5f, 1.5f);
        //meteorInstance.transform.localScale = Vector3.one * randomScale;

        // Configure the Meteor script on the instance
        Meteor meteorScript = meteorInstance.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.drift = new Vector2(Random.Range(-1f, 1f), 0);
            meteorScript.rotationSpeed = Random.Range(-90f, 90f);
            meteorScript.wobbleAmount = Random.Range(0.5f, 1.5f);
            meteorScript.wobbleSpeed = Random.Range(1f, 3f);
        }
    }

}
