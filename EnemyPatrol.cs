using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;         // Speed at which enemy moves
    public Transform pointA;             // First patrol point
    public Transform pointB;             // Second patrol point

    private Vector2 targetPosition;      // The current target position
    private float reachThreshold = 0.1f; // Threshold to determine when to switch targets

    void Start()
    {
        // Set the initial target position to point A
        targetPosition = pointA.position;
        // Ensure the enemy starts facing the correct direction
        UpdateRotation();
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // If the enemy reaches the target position, switch to the other target
        if (Vector2.Distance(transform.position, targetPosition) < reachThreshold)
        {
            SwitchTarget();
        }
    }

    void SwitchTarget()
    {
        if (targetPosition == (Vector2)pointA.position)
        {
            targetPosition = pointB.position;
        }
        else
        {
            targetPosition = pointA.position;
        }
        UpdateRotation(); // Update the rotation based on the new target position
    }

    void UpdateRotation()
    {
        // Face the correct direction based on the target position
        if (targetPosition == (Vector2)pointA.position)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);  // Face right at pointA
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);  // Face left at pointB
        }
    }
}
