using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    // Player is attacking
    [HideInInspector] public bool doAttack;

    // Speed at which the player moves.
    public float speed = 4;
    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

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

    void Update()
    {
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical"); // setup v variables as our vertical input axis


        if (InputMapToCircular)
        {
            // make coordinates circular
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

        if (Mathf.Abs(h) > 0 && v == 0)
        {
            v = 0.05f;
        }


        //do some filtering of our input as well as clamp to a speed limit
        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v,
            Time.deltaTime * forwardInputFilter), -speed, speed);

        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h,
            Time.deltaTime * turnInputFilter);

        Forward = filteredForwardInput;
        Turn = filteredTurnInput;
    }

}
