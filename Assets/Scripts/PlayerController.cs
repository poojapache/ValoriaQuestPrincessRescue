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

    //Number of gems collected so far
    public int noOfGems;

    //Variable for key and energy count on Canvas
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI GemCountText;
    public TextMeshProUGUI energyCountText;
    public GameObject doorKeyNeededText;

    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public bool InputMapToCircular = true;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    //Variables for movement
    public float Forward
    {
        get;
        private set;
    }

    public float Turn
    {
        get;
        private set;
    }

    // Initialize; Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        noOfKeys = 0;
        energyLevel = 0;
        SetKeyCountText();
        SetGemCountText();
        SetEnergyCountText();
    }


    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        //Debug.Log("moved");
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

    // Set the energy level
    void SetEnergyCountText()
    {
        energyCountText.text = energyLevel.ToString();
    }

    //Set the number of gems collected on canvase
    void SetGemCountText()
    {
        GemCountText.text = noOfGems.ToString();
    }

    // Collision with trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        //if (other.gameObject.CompareTag("KeyCollectible"))
        //{
        //    // Deactivate the collided object (making it disappear).
        //    other.gameObject.SetActive(false);
        //    noOfKeys += 1;
        //    doorKeyNeededText.SetActive(false);
        //    SetKeyCountText();
        //}
        //if (other.gameObject.CompareTag("EnergyCollectible"))
        //{
        //    // Deactivate the collided object (making it disappear).
        //    Debug.Log(other.gameObject.tag);
        //    other.gameObject.SetActive(false);
        //    energyLevel += 10;
        //    SetEnergyCountText();
        //}
        if (other.gameObject.CompareTag("EnergyPotionCollectible"))
        {
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
            other.gameObject.SetActive(false);
            energyLevel += 50;
            SetEnergyCountText();
        }
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("GemCollectable"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            noOfGems += 1;
            SetGemCountText();
        }
        if (other.gameObject.CompareTag("Ghost"))
        {
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
            other.gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.gameObject.CompareTag("KeyCollectible"))
        {
            //collision sound here
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            noOfKeys += 1;
            //doorKeyNeededText.SetActive(false);
            SetKeyCountText();
        }
        if (other.gameObject.CompareTag("EnergyCollectible"))
        {
            //collision sound here
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
<<<<<<< Updated upstream
            other.gameObject.SetActive(false);
=======
            other.transform.parent.gameObject.SetActive(false);
>>>>>>> Stashed changes
            energyLevel += 10;
            SetEnergyCountText();
        }
    }



    void Update()
    {
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical"); // setup v variables as our vertical input axis


        if (InputMapToCircular)
        {
            // make coordinates circular
            //based on http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            h = h * Mathf.Sqrt(1f - 0.5f * v * v);
            v = v * Mathf.Sqrt(1f - 0.5f * h * h);

        }

        if (Input.GetKey(KeyCode.Q))
        {
            h = -0.5f;
            v += 0.1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            h = 0.5f;
            v += 0.1f;
        }


        //do some filtering of our input as well as clamp to a speed limit
        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v,
            Time.deltaTime * forwardInputFilter), -speed, speed);

        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h,
            Time.deltaTime * turnInputFilter);

        Forward = filteredForwardInput;
        Turn = filteredTurnInput;
    }

    public void CollectKey()
    {
        //other.gameObject.SetActive(false);
        noOfKeys += 1;
        //doorKeyNeededText.SetActive(false);
        SetKeyCountText();
    }
}
