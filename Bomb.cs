using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator animator; // Reference to the Animator
    public float explosionDelay = 2f; // Delay before the bomb explodes
    private HealthManager healthManager; // HealthManager reference

    void Start()
    {
        // Get the Animator component attached to the bomb
        animator = GetComponent<Animator>();

        // Find the HealthManager in the scene
        healthManager = FindObjectOfType<HealthManager>();

        // Start the explosion timer
        StartCoroutine(ExplosionTimer());
    }

    private IEnumerator ExplosionTimer()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(explosionDelay);

        // Trigger the explosion animation
        Explode();
    }

    public void Explode()
    {
        animator.SetTrigger("explode"); // Assuming you have a trigger called "explode"

        // Detect collisions with objects in the explosion radius
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 2f); // Change radius as needed

        foreach (Collider2D hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player detected");
                if (healthManager != null)
                {
                    Debug.Log("Applying damage...");
                    healthManager.TakeDamage(); // Call TakeDamage method on the health manager
                }
            }
        }

        // Optionally, destroy the bomb after the animation plays
        Destroy(gameObject, 1f); // Adjust the time based on the animation length
    }

    // private void OnDrawGizmosSelected()
    // {
    //     // Draw a red sphere in the Scene view to visualize the explosion radius
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 5f); // Adjust radius to match the overlap radius
    // }
}
