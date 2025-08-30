using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    //movement
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb2;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    //minimap
    [Header("UI Elements")]
    public RectTransform mapImage;    // The minimap background
    public RectTransform playerIcon;  // The player’s dot

    [Header("Player Reference")]
    public Transform player;          // This minimap’s player

    [Header("World Size")]
    public float halfWorldWidth = 10f;   // match LoopingWorld
    public float halfWorldHeight = 5f;   // set to 0 if no vertical looping
    public bool canJump = true;

     void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move left/right
        float moveInput = Input.GetAxisRaw("Horizontal_P2");
        rb2.linearVelocity = new Vector2(moveInput * moveSpeed, rb2.linearVelocity.y);

        // Check if on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Normalize player position (-1..+1 relative to world bounds)
        float normX = Mathf.Clamp(player.position.x / halfWorldWidth, -1f, 1f);
        float normY = Mathf.Clamp(player.position.y / halfWorldHeight, -1f, 1f);

        // Convert to minimap rect space
        float mapHalfW = mapImage.rect.width * 0.5f;
        float mapHalfH = mapImage.rect.height * 0.5f;

        float posX = normX * mapHalfW;
        float posY = normY * mapHalfH;

        playerIcon.localPosition = new Vector3(posX, posY, 0f);

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && canJump)
        {
            rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, jumpForce);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check in Scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
