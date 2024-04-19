using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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
    public GameObject gameInfo;
    private TextMeshProUGUI gameInfoText;

    public GameObject keyImage;
    public GameObject energyImage;
    public GameObject gemImage;

    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public bool InputMapToCircular = true;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    private AudioSource audioSource;
    public AudioClip[] audioClips;

    public GameObject gameLostGameObject;
    public GameObject gameWonGameObject;
    public GameObject respawnObject;
    public TextMeshProUGUI respawnMessage;

    public GameObject backgroundAudio;

    public Animator[] gameDoorAnimator;
    public Animator[] dummyDoorAnimator;

    [HideInInspector] public int ghost1Energy = 60;
    [HideInInspector] public int ghost2Energy = 100;
    [HideInInspector] public int ghost2Gems = 4;

    public GameObject bigGhost;
    public GameObject malePlayer;
    public GameObject femalePlayer;

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
        if(CharcaterSelector.character == 1)
        {
            malePlayer.SetActive(false);
            femalePlayer.SetActive(true);
        }
        else
        {
            malePlayer.SetActive(true);
            femalePlayer.SetActive(false);
        }
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameInfoText = gameInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
            StartCoroutine(UIAnimationGhostCoroutine(energyImage));
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
            StartCoroutine(UIAnimationGhostCoroutine(gemImage));
            SetGemCountText();
        }
        if (other.gameObject.CompareTag("Ghost"))
        {
            
            if (other.gameObject.GetComponent<PatrolandRunaway>() != null && energyLevel >= ghost1Energy)
            {
                doAttack = true;
                CollectKey();
                // Deactivate the collided object (making it disappear).
                Debug.Log(other.gameObject.tag);
                StartCoroutine(KillGhostCoroutine(other.gameObject));
            }
            else if (other.gameObject.GetComponent<PatrolAndChase>() != null && energyLevel >= ghost2Energy)
            {
                //doAttack = true;
                //CollectKey();
                //// Deactivate the collided object (making it disappear).
                //Debug.Log(other.gameObject.tag);
                //StartCoroutine(KillGhostCoroutine(other.gameObject));
                GameLost();
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
            CollectKey();
            if(noOfKeys == 7)
            {
                StartCoroutine(KillGhostCoroutine(bigGhost));
            }
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("EnergyCollectible"))
        {
            //collision sound here
            audioSource.clip = audioClips[1];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            Debug.Log(other.gameObject.tag);

            other.transform.parent.gameObject.SetActive(false);
            StartCoroutine(UIAnimationGhostCoroutine(energyImage));
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

    public void SetEnergyWarningText(int energy)
    {
        if (energy == 2)
        {
            gameInfo.SetActive(true);
            gameInfoText.GetComponent<TextMeshProUGUI>().text = "You need at least " + ghost2Energy + " energy and " + ghost2Gems + " gems to defeat the ghost!";
        }
        else
        {
            gameInfo.SetActive(true);
            gameInfoText.text = "You need at least " + ghost1Energy + " energy level to defeat the ghost!";
        }
    }

    public void ClearGameInfoText()
    {
        gameInfo.SetActive(false);
    }
    public void CollectKey()
    {//collision sound here
        audioSource.clip = audioClips[0];
        audioSource.Play();
        noOfKeys += 1;
        StartCoroutine(UIAnimationGhostCoroutine(keyImage));
        SetKeyCountText();
        SetDoorJam();
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

    public void DoorOpen(Animator animator)
    {
        animator.SetInteger("doorVal", 2);
        audioSource.clip = audioClips[7];
        audioSource.Play();
    }

    void SetDoorJam()
    {
        gameDoorAnimator[noOfKeys - 1].SetInteger("doorVal", 1);
        if (noOfKeys == 4)
        {
            dummyDoorAnimator[0].SetInteger("doorVal", 1);
            dummyDoorAnimator[1].SetInteger("doorVal", 1);
        }
        else if (noOfKeys == 5) dummyDoorAnimator[2].SetInteger("doorVal", 1);
        else if (noOfKeys == 6) dummyDoorAnimator[3].SetInteger("doorVal", 1);

        //undo prev door anim
        if (noOfKeys > 1) gameDoorAnimator[noOfKeys - 2].SetInteger("doorVal", 0);
        if (noOfKeys == 5)
        {
            dummyDoorAnimator[0].SetInteger("doorVal", 0);
            dummyDoorAnimator[1].SetInteger("doorVal", 0);
        }
        else if (noOfKeys == 6) dummyDoorAnimator[2].SetInteger("doorVal", 0);
        else if (noOfKeys == 7) dummyDoorAnimator[3].SetInteger("doorVal", 0);
    }

    public void PlayDummyDoorAudio()
    {
        //collision sound here
        audioSource.clip = audioClips[8];
        audioSource.Play();
    }

    private IEnumerator KillGhostCoroutine(GameObject ghost)
    {
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = audioClips[4];
        audioSource.Play();
        ghost.SetActive(false);
    }

    private IEnumerator UIAnimationGhostCoroutine(GameObject icon)
    {
        icon.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);
        icon.GetComponent<Animation>().Stop();
    }
}
