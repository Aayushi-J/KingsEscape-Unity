using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed1 = 5f; // Original speed for one set of keys
    public float moveSpeed2 = 5f; // Original speed for another set of keys
    public float maxSpeed = 12f; // Maximum speed limit
    public float minSpeed = 1f; // Minimum speed limit
    public float speedChangeRate = 0.1f; // Rate of gradual increase/decrease
    private float currentSpeed1;
    private float currentSpeed2;
    public float jumpForce = 5f;
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb.freezeRotation = true;

        // Initialize the current speeds with the original values
        currentSpeed1 = moveSpeed1;
        currentSpeed2 = moveSpeed2;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleSpeedControl(); // New function to handle I/O key speed control
        transform.localScale = originalScale;
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        // Check for different keys to set different speeds
        if (Input.GetKey(KeyCode.LeftArrow)) // Move with speed1 when Left Arrow is pressed
        {
            moveInput = -currentSpeed1;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Move with speed1 when Right Arrow is pressed
        {
            moveInput = currentSpeed1;
        }
        else if (Input.GetKey(KeyCode.A)) // Move with speed2 when A is pressed
        {
            moveInput = -currentSpeed2;
        }
        else if (Input.GetKey(KeyCode.D)) // Move with speed2 when D is pressed
        {
            moveInput = currentSpeed2;
        }

        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        animator.SetBool("isWalking", moveInput != 0);

        // Flip character based on movement
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void HandleSpeedControl()
    {
        // Gradually increase speed when "I" is pressed
        if (Input.GetKey(KeyCode.I))
        {
            currentSpeed1 = Mathf.Min(currentSpeed1 + speedChangeRate, maxSpeed);
            currentSpeed2 = Mathf.Min(currentSpeed2 + speedChangeRate, maxSpeed);
        }
        // Gradually decrease speed when "O" is pressed
        else if (Input.GetKey(KeyCode.O))
        {
            currentSpeed1 = Mathf.Max(currentSpeed1 - speedChangeRate, minSpeed);
            currentSpeed2 = Mathf.Max(currentSpeed2 - speedChangeRate, minSpeed);
        }
        // Reset to original speed if none of these keys are pressed
        else
        {
            currentSpeed1 = moveSpeed1;
            currentSpeed2 = moveSpeed2;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0)) // 1 is the right mouse button
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded when colliding with objects tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // If the player leaves the ground, set isGrounded to false
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("escape"))
        {
            animator.SetTrigger("Runout");
            StartCoroutine(ChangeSceneAfterDelay(2f)); // Set delay to 1 second (adjust as needed)
        }

        if (other.CompareTag("bomb"))
        {
            Debug.Log("Player ded");
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                StartCoroutine(ChangeColorTemporarily(spriteRenderer, new Color(1f, 0.5f, 0.5f), 1f)); // 1 second delay
            }

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                float knockbackForce = 5f;

                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            Debug.Log("Player hit by bomb, color changed, and knocked back");
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            SceneManager.LoadScene("Congrats");
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
    }

    IEnumerator ChangeColorTemporarily(SpriteRenderer spriteRenderer, Color newColor, float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = newColor;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;
    }
}