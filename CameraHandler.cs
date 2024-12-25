using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float smoothSpeed = 0.125f; // Smoothness factor
    public Vector3 offset; // Offset from the player

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset; // Desired camera position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smooth transition
        transform.position = smoothedPosition; // Update camera position
    }
}
