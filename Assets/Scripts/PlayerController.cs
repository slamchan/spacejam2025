using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public int playerId = 1;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool canJump = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Input")]
    public string horizontalAxis = "Horizontal_P1";
    public KeyCode jumpKey = KeyCode.W;

    [Header("Minimap")]
    public RectTransform mapImage;
    public RectTransform playerIcon;
    public Transform player;  // The world transform of this player
    public float halfWorldWidth = 10f;
    public float halfWorldHeight = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw(horizontalAxis);
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Minimap update
        UpdateMinimap();
    }

    private void UpdateMinimap()
    {
        float normX = Mathf.Clamp(player.position.x / halfWorldWidth, -1f, 1f);
        float normY = Mathf.Clamp(player.position.y / halfWorldHeight, -1f, 1f);

        float mapHalfW = mapImage.rect.width * 0.5f;
        float mapHalfH = mapImage.rect.height * 0.5f;

        playerIcon.localPosition = new Vector3(normX * mapHalfW, normY * mapHalfH, 0f);
    }
}
