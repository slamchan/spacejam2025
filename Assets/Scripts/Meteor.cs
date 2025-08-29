using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Stats")]
    public float fallSpeed = 5f;        // Base fall speed
    public int hp = 3;                  // Meteor health
    public int power = 1;               // How much shield it reduces
    public bool destroyOnImpact = true;

    [Header("Collectible On Destroy")]
    public GameObject collectiblePrefab; // Optional resource spawned on destruction
    public string resourceType;          // Optional
    public int amount = 1;               // Optional

    [Header("Movement")]
    public Vector2 drift = Vector2.zero; // Add some sideways movement
    public float rotationSpeed = 0f;     // Optional spinning


    public float wobbleAmount = 1f;
    public float wobbleSpeed = 2f;
    private float wobbleTimer = 0f;


    private void Update()
    {
        wobbleTimer += Time.deltaTime;
        float xOffset = Mathf.Sin(wobbleTimer * wobbleSpeed) * wobbleAmount;
        Vector2 movement = new Vector2(xOffset + drift.x, -fallSpeed) * Time.deltaTime;
        transform.Translate(movement);

        // Optional rotation
        if (rotationSpeed != 0)
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }




    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            DestroyMeteor();
        }
    }

    private void DestroyMeteor()
    {
        // Spawn collectible if assigned
        if (collectiblePrefab != null)
        {
            GameObject collectible = Instantiate(collectiblePrefab, transform.position, Quaternion.identity);

            // Set type and amount if using dynamic system
            var colScript = collectible.GetComponent<Collectible>();
            if (colScript != null)
            {
                colScript.resourceType = resourceType;
                colScript.amount = amount;
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        // Example: Reduce shield if hitting a player
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {

            Debug.Log($"Player {player} does not have enough resources!");

            if (player.shield > 0)
            {
                player.shield -= power;
                if (player.shield < 0) player.shield = 0;
            }

            DestroyMeteor();
        }

        // Destroy on hitting ground
        if (collision.CompareTag("Ground"))
        {
            DestroyMeteor();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            DestroyMeteor();
        }
    }
}


