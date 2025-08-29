using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallSpeed = 5f;          // Base speed
    public int hp = 3;                     // Meteor health
    public int power = 1;                  // Shield damage
    public GameObject collectiblePrefab;   // Optional collectible
    public string resourceType;
    public int amount = 1;

    private Vector2 direction;  

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;       // Ensure it’s a unit vector
    }

    private void Update()
    {
        // Move in straight line along the assigned direction
        transform.Translate(direction * fallSpeed * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.shield > 0)
            {
                player.shield -= power;
                if (player.shield < 0) player.shield = 0;
            }
            DestroyMeteor();
        }

        if (collision.collider.CompareTag("Ground"))
        {
            DestroyMeteor();
        }
    }
}


