using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Animator animator;
    private HealthManager healthManager; // HealthManager reference
    public bool isPlayerDead; // Flag to track if the player is dead
    private Animator playerAnimator;

    void Start()
    {
        // Get the Animator component attached to the bomb
        animator = GetComponent<Animator>();

        // Find the HealthManager in the scene
        healthManager = FindObjectOfType<HealthManager>();
        isPlayerDead = false; // Initialize the player dead flag to false
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is tagged as "Player" and if the player is not already dead
        if (collision.gameObject.CompareTag("Player") && !isPlayerDead)
        {
            // Trigger bomb explosion
            Explode();
            isPlayerDead = true;

            // Get the player's Animator component
            playerAnimator = collision.GetComponent<Animator>();

            // Apply damage to the player using the health manager
            if (healthManager != null)
            {
                Debug.Log("Applying damage to player...");
                healthManager.TakeDamage(); // Call TakeDamage method on the health manager
            }

            // Apply knockback force to the player
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized; // Direction away from bomb
                float knockbackForce = 10f; // Adjust the knockback force
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // Apply knockback
            }

            Debug.Log("Player hit by bomb and knocked back");
        }
    }

    void Explode()
    {
        Debug.Log("Player entered, bomb exploding!");
        // Trigger the explosion animation
        animator.SetTrigger("New Trigger");

        // Destroy the bomb after a delay (e.g., 2 seconds)
        Destroy(gameObject, 2f);
    }
}
