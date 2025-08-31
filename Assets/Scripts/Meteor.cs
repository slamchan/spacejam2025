using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallSpeed = 5f;          // Base speed
    public int hp = 3;                     // Meteor health
    public int power = 1;                  // Shield damage
    public GameObject collectiblePrefab;   // Optional collectible
    public string resourceType;
    public int amount = 1;
    private float spinSpeed; // rotation speed

    public Transform spriteTransform; // assign the child in Inspector

    private Vector2 direction;

    private void Awake()
    {
        // Find the first child that has a SpriteRenderer
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            spriteTransform = sr.transform;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;       // Ensure it’s a unit vector
    }

    public void SetSpin(float spin)
    {
        spinSpeed = spin;
    }

    private void Update()
    {
        // Move in straight line along the assigned direction
        transform.Translate(direction * fallSpeed * Time.deltaTime, Space.World);
        // Only spin the sprite child
        if (spriteTransform != null)
        {
            spriteTransform.Rotate(0f, 0f, spinSpeed * Time.deltaTime, Space.Self);
        }
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
        SoundManager.Instance.PlayDestructionSound();

        // Spawn collectible if assigned
        if (collectiblePrefab != null && Random.value <= 0.01f)
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

        // Destroy the meteor object
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            //todo
            DestroyMeteor();
        }

        if (collision.collider.CompareTag("Ground"))
        {
            DestroyMeteor();
        }
    }
}


