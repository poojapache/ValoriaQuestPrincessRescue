using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;
    public LayerMask obstacleMask; // Define layers that represent obstacles (e.g., walls)
    public float yPositionOffset = 3.25f; // Offset to move the camera higher when near a wall

    private void Start()
    {
        target = GameObject.FindWithTag("SceneController").GetComponent<SceneController>().player.transform;
    }
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + target.rotation * locationOffset;

        // Cast a ray from the camera towards the target (player).
        RaycastHit hit;
        if (Physics.Raycast(target.position, desiredPosition - target.position, out hit, Vector3.Distance(target.position, desiredPosition), obstacleMask))
        {
            // If the ray hits an obstacle, move the camera to the other side of the obstacle.
            desiredPosition = hit.point + hit.normal * 0.1f; // Add a small offset to avoid clipping into the wall

            // Adjust y position to move the camera higher when near a wall
            desiredPosition.y = yPositionOffset;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion desiredRotation = target.rotation * Quaternion.Euler(rotationOffset);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        transform.rotation = smoothedRotation;
    }
}
