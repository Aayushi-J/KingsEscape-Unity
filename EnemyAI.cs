using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    private Vector3 originalPosition; // To store the enemy's original position
    public float moveSpeed = 2f; // Speed of the enemy movement towards the player

    // Variables for bomb spawning and throwing
    public GameObject bombPrefab; // The bomb prefab to be instantiated
    public Transform spawnPoint;  // The point where the bomb will spawn
    public float throwForce = 10f; // The force with which the bomb will be thrown
    public float bombThrowDelay = 2f; // Delay between bomb throws

    // Player detection variables
    public Transform player; // Reference to the player's transform
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public float attackRange = 3f; // Range within which the enemy throws the bomb

    private bool canThrowBomb = true; // Flag to check if the enemy can throw a bomb

    void Start()
    {
        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();
        originalPosition = transform.position; // Store the original position of the enemy
    }

    void Update()
    {
        // Detect the player and handle movement and attack
        DetectAndAttackPlayer();
    }

    void DetectAndAttackPlayer()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Move towards the player if not within attack range
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                // Trigger the "Attack" animation if within attack range
                if (canThrowBomb)
                {
                    animator.SetTrigger("isAttack");
                    StartCoroutine(ThrowBombWithDelay());
                }
            }
        }
        else
        {
            // Move back to the original position if the player is out of detection range
            MoveBackToOriginalPosition();
        }
    }

    void MoveTowardsPlayer()
    {
        // Move the enemy towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Change the enemy's facing direction based on the player's position
        if (player.position.x < transform.position.x)
        {
            // Player is to the left
            transform.localScale = new Vector3(-1, 1, 1); // Flip to face left
        }
        else
        {
            // Player is to the right
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
    }

    void MoveBackToOriginalPosition()
    {
        // Move the enemy back to its original position
        if (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            Vector3 direction = (originalPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Change the enemy's facing direction based on its position relative to the original position
            if (originalPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip to face left
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1); // Face right
            }
        }
    }

    private IEnumerator ThrowBombWithDelay()
    {
        canThrowBomb = false; // Set the flag to prevent throwing bombs repeatedly

        // Call the method to spawn and throw the bomb
        SpawnAndThrowBomb();

        // Wait for the specified delay before allowing the enemy to throw another bomb
        yield return new WaitForSeconds(bombThrowDelay);

        canThrowBomb = true; // Allow throwing bombs again
    }

    // This function will be called via an Animation Event in the attack animation
    public void SpawnAndThrowBomb()
    {
        // Instantiate the bomb at the spawn point's position
        GameObject bomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the Rigidbody2D component of the bomb to apply force (for 2D games)
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();

        // Apply a force to throw the bomb (direction based on enemy's facing direction)
        rb.AddForce(new Vector2(transform.localScale.x * throwForce, 5f), ForceMode2D.Impulse);
        rb.gravityScale = 2f; // Faster falling
        rb.drag = 3f;

        // Get the Bomb component and optionally set the explosion method
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            // The timer for explosion is handled within the Bomb script
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a green sphere to visualize the detection range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw a red sphere to visualize the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("axe"))
        {
            Destroy(gameObject);
            FindObjectOfType<ScoreManager>().IncreaseScore();
            Debug.Log("attacked");
        }
    }
}
