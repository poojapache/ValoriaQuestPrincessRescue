using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    //Store the key count and energy count
    public int noOfKeys;
    public int energyLevel;

    //Variable for key and energy count on Canvas
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI energyCountText;
    public GameObject doorKeyNeededText;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        noOfKeys = 0;
        energyLevel = 0;
        SetKeyCountText();
        SetEnergyCountText();
    }


    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        Debug.Log("moved");
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    //Set the number of keys
    void SetKeyCountText()
    {
        keyCountText.text = noOfKeys.ToString();
    }

    //Set the energy level
    void SetEnergyCountText()
    {
        energyCountText.text = energyLevel.ToString();
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("KeyCollectible"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            noOfKeys += 1;
            doorKeyNeededText.SetActive(false);
            SetKeyCountText();
        }
        if (other.gameObject.CompareTag("EnergyCollectible"))
        {
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
            other.gameObject.SetActive(false);
            energyLevel += 10;
            SetEnergyCountText();
        }
    }
}