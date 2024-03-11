using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player GameObject.
    //public GameObject player;

    //// The distance between the camera and the player.
    //private Vector3 offset;
    //public float xOffsetAngle = 0f;

    //// Start is called before the first frame update.
    //void Start()
    //{
    //    // Calculate the initial offset between the camera's position and the player's position.
    //    offset = transform.position - player.transform.position;
    //}

    //// LateUpdate is called once per frame after all Update functions have been completed.
    //void LateUpdate()
    //{
    //    // Update the position to follow the player.
    //    transform.position = player.transform.position + offset;

    //    // Rotate the camera to match the player's rotation with an additional offset for x rotation.
    //    Quaternion playerRotation = Quaternion.Euler(player.transform.eulerAngles.x + xOffsetAngle, player.transform.eulerAngles.y, player.transform.eulerAngles.z);
    //    transform.rotation = Quaternion.LookRotation(playerRotation * Vector3.forward, Vector3.up);
    //}

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + target.rotation * locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion desiredrotation = target.rotation * Quaternion.Euler(rotationOffset);
        Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
        transform.rotation = smoothedrotation;
    }
}