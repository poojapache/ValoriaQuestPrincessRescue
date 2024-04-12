using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewPlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    // Player is attacking
    public bool doAttack;

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

    public int doorCount;

    //Variable for key and energy count on Canvas
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI GemCountText;
    public TextMeshProUGUI energyCountText;
    public TextMeshProUGUI gameInfoText;

    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public bool InputMapToCircular = true;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    private AudioSource audioSource;
    public AudioClip[] audioClips;

    public GameObject gameLostGameObject;
    public GameObject gameWonGameObject;
    public GameObject backgroundAudio;

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
        audioSource = GetComponent<AudioSource>();
        noOfKeys = 0;
        energyLevel = 0;
        noOfGems = 0;
        doorCount = 0;
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
        if (other.gameObject.CompareTag("EnergyPotionCollectible"))
        {
            audioSource.clip = audioClips[3];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
            other.gameObject.SetActive(false);
            energyLevel += 50;
            SetEnergyCountText();
        }
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("GemCollectable"))
        {
            audioSource.clip = audioClips[2];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            noOfGems += 1;
            SetGemCountText();
        }
        if (other.gameObject.CompareTag("Ghost"))
        {
            if (energyLevel >= other.gameObject.GetComponent<PatrolandRunaway>().energyLevel)
            {
                doAttack = true;
                Debug.Log("set attack");
                audioSource.clip = audioClips[4];
                audioSource.Play();
                // Deactivate the collided object (making it disappear).
                Debug.Log(other.gameObject.tag);
                other.gameObject.SetActive(false);
            }
            else GameLost();

        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            audioSource.clip = audioClips[4];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);
            other.gameObject.SetActive(false);
            GameLost();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.gameObject.CompareTag("KeyCollectible"))
        {
            //collision sound here
            audioSource.clip = audioClips[0];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            noOfKeys += 1;
            //doorKeyNeededText.SetActive(false);
            SetKeyCountText();
        }
        if (other.gameObject.CompareTag("EnergyCollectible"))
        {
            //collision sound here
            audioSource.clip = audioClips[1];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);

            other.transform.parent.gameObject.SetActive(false);

            energyLevel += 10;
            SetEnergyCountText();
        }
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        h = h * Mathf.Sqrt(1f - 0.5f * v * v);
        v = v * Mathf.Sqrt(1f - 0.5f * h * h);

        if (Input.GetKey(KeyCode.Q))
        {
            h = -0.5f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            h = 0.5f;
        }

        if (Mathf.Abs(h) > 0 && v == 0)
        {
            v = 0.05f;
        }

        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v, Time.deltaTime * forwardInputFilter), -speed, speed);
        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h, Time.deltaTime * turnInputFilter);

        Forward = filteredForwardInput;
        Turn = filteredTurnInput;
    }

    public void SetEnergyWarningText(int energy)
    {
        if (energy == 100)
        {

            gameInfoText.text = "You need at least 100 energy and 4 gems to defeat the ghost!";
        }
        else
        {

            gameInfoText.text = "You need at least " + energy + " energy level to defeat the ghost!";
        }
    }

    public void ClearGameInfoText()
    {
        gameInfoText.text = "";
    }
    public void CollectKey()
    {//collision sound here
        audioSource.clip = audioClips[0];
        audioSource.Play();
        noOfKeys += 1;
        SetKeyCountText();
    }

    public void GameWon()
    {
        audioSource.clip = audioClips[5];
        audioSource.Play();
        Time.timeScale = 0f;
        gameWonGameObject.SetActive(true);
        backgroundAudio.SetActive(false);
    }

    public void GameLost()
    {
        audioSource.clip = audioClips[6];
        audioSource.Play();
        Time.timeScale = 0f;
        gameLostGameObject.SetActive(true);
        backgroundAudio.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}
