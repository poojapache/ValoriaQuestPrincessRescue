using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    //Store the key count and energy count
    public int noOfKeys;
    public int energyLevel;

    //Number of gems collected so far
    public int noOfGems;

    //Variable for key and energy count on Canvas
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI GemCountText;
    public TextMeshProUGUI energyCountText;
    public GameObject gameInfo;
    private TextMeshProUGUI gameInfoText;

    public GameObject keyImage;
    public GameObject energyImage;
    public GameObject gemImage;

    private AudioSource audioSource;
    public AudioClip[] audioClips;

    public GameObject gameLostGameObject;
    public GameObject gameWonGameObject;
    public GameObject respawnObject;
    public TextMeshProUGUI respawnMessage;

    public GameObject levelWonObject;
    public TextMeshProUGUI levelWonMessage;

    public GameObject gameWonObject;
    public TextMeshProUGUI gameWonMessage;

    public GameObject backgroundAudio;

    public Animator[] gameDoorAnimator;
    public Animator[] dummyDoorAnimator;

    [HideInInspector] public int ghost1Energy = 60;
    [HideInInspector] public int ghost2Energy = 100;
    [HideInInspector] public int ghost2Gems = 4;

    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerMovementController playerMovementController;
    private SceneController sceneController;

    [HideInInspector] public bool collectedSoftStarGem;
    [HideInInspector] public bool collectedHeartGem;
    [HideInInspector] public bool collectedCubiodGem;
    [HideInInspector] public bool collectedHexagonGem;
    public GameObject softStarEnemyParent;
    public GameObject heartEnemyParent;
    public GameObject cubieEnemyParent;
    public bool collectedPotion;
    public GameObject bigGhost;

    // Initialize; Start is called before the first frame update.
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovementController = GetComponent<PlayerMovementController>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        gameInfoText = gameInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        sceneController = GameObject.FindWithTag("SceneController").GetComponent<SceneController>();
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            noOfGems = sceneController.gems;
            noOfKeys = sceneController.keys;
            energyLevel = sceneController.energy;
        }
        
        collectedSoftStarGem = false;
        collectedHeartGem = false;
        collectedCubiodGem = false;
        collectedHexagonGem = false;

        SetKeyCountText();
        SetGemCountText();
        SetEnergyCountText();
    }


    //Set the number of keys
    void SetKeyCountText()
    {
        keyCountText.text = noOfKeys.ToString();
        Debug.Log("set keys to " + noOfKeys);
    }

    // Set the energy level
    void SetEnergyCountText()
    {
        energyCountText.text = energyLevel.ToString();
        if (energyLevel < 50 && collectedPotion) energyCountText.color = Color.red;
        else energyCountText.color = Color.white;
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
            collectedPotion = true;
            StartCoroutine(UIAnimationCoroutine(energyImage));
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
            StartCoroutine(UIAnimationCoroutine(gemImage));
            SetGemCountText();
            SetGemStatus(other.gameObject.name);
            if (noOfGems == 5 && gameInfo.activeSelf) gameInfo.SetActive(false);
            if (noOfGems == 9 && gameInfo.activeSelf) gameInfo.SetActive(false);

        }
        if (other.gameObject.CompareTag("Ghost"))
        {

            if (other.gameObject.GetComponent<PatrolandRunaway>() != null && energyLevel >= ghost1Energy)
            {
                playerMovementController.doAttack = true;
                CollectKey();
                // Deactivate the collided object (making it disappear).
                Debug.Log(other.gameObject.tag);
                StartCoroutine(KillGhostCoroutine(other.gameObject));
            }
            else if (other.gameObject.GetComponent<PatrolAndChase>() != null && energyLevel >= ghost2Energy)
            {
                GameLost();
            }
            else GameLost();

        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            audioSource.clip = audioClips[4];
            audioSource.Play();
            // Deactivate the collided object (making it disappear).
            energyLevel -= 10; ;
            StartCoroutine(UIAnimationCoroutine(energyImage));
            SetEnergyCountText();
            if (energyLevel == 0 && collectedPotion) GameLost();
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.gameObject.CompareTag("KeyCollectible"))
        {
            CollectKey();
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
            StartCoroutine(UIAnimationCoroutine(energyImage));
            energyLevel += 10;
            SetEnergyCountText();
        }
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

    public void SetGemWarningText(int gems)
    {
        gameInfo.SetActive(true);
        gameInfoText.GetComponent<TextMeshProUGUI>().text = "You need " + gems + " gems to complete this level!";
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
        StartCoroutine(UIAnimationCoroutine(keyImage));
        SetKeyCountText();
        SetDoorJam();
        CheckKillGhost();
        if(noOfKeys == 9)
        {
            Destroy(bigGhost);
            PlayKillGhostAudio();
        }
    }


    public void GameLost()
    {
        audioSource.clip = audioClips[6];
        audioSource.Play();
        Time.timeScale = 0f;
        gameLostGameObject.SetActive(true);
        backgroundAudio.SetActive(false);
        DisableInput();
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
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        //level 1
        if (levelIndex == 1)
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
        }
        //level 2
        else
        {
            //unlock doors
            gameDoorAnimator[noOfKeys - 7].SetInteger("doorVal", 1);
            if (noOfKeys == 7) dummyDoorAnimator[0].SetInteger("doorVal", 1);

            //undo prev door anim
            if (noOfKeys > 7) gameDoorAnimator[noOfKeys - 8].SetInteger("doorVal", 0);
            if (noOfKeys == 8) dummyDoorAnimator[0].SetInteger("doorVal", 0);
        }

    }

    public void PlayDummyDoorAudio()
    {
        //collision sound here
        audioSource.clip = audioClips[8];
        audioSource.Play();
    }

    public void PlayLevelWonAudio()
    {
        //collision sound here
        audioSource.clip = audioClips[5];
        audioSource.Play();
    }

    private IEnumerator KillGhostCoroutine(GameObject ghost)
    {
        yield return new WaitForSeconds(0.2f);
        PlayKillGhostAudio();
        ghost.SetActive(false);
    }

    public void PlayKillGhostAudio()
    {
        audioSource.clip = audioClips[4];
        audioSource.Play();
    }

    private IEnumerator UIAnimationCoroutine(GameObject icon)
    {
        icon.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);
        icon.GetComponent<Animation>().Stop();
    }

    public void DisableInput()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        playerInput.enabled = false;
    }

    public void EnableInput()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        playerInput.enabled = true;
    }

    private IEnumerator RespawnControllerCoroutine(Animator animator, GameObject keyInfoGameObject, GameObject respawnRoomLight, Transform respawnTransform)
    {
        yield return new WaitForSeconds(1.1f);
        //audio
        PlayDummyDoorAudio();
        //disable player rigid body
        DisableInput();
        //show message on screen
        respawnObject.SetActive(true);
        respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 5... ";
        yield return new WaitForSeconds(0.5f);
        respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 4... ";
        yield return new WaitForSeconds(0.5f);
        respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 3...";
        yield return new WaitForSeconds(0.5f);
        respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 2...";
        yield return new WaitForSeconds(0.5f);
        respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 1...";
        yield return new WaitForSeconds(0.5f);
        //close door
        animator.SetInteger("doorVal", 3);
        keyInfoGameObject.SetActive(true);
        //adjust lighting
        respawnRoomLight.SetActive(false);
        // respawn
        transform.position = respawnTransform.position;
        transform.rotation = respawnTransform.rotation;
        //resume game
        EnableInput();
        //hide respawn message
        respawnObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        respawnRoomLight.SetActive(true);
    }

    public void Respawn(Animator animator, GameObject keyInfoGameObject, GameObject respawnRoomLight, Transform respawnTransform)
    {
        StartCoroutine(RespawnControllerCoroutine(animator, keyInfoGameObject, respawnRoomLight, respawnTransform));
    }

    private IEnumerator LevelWonCoroutine()
    {

        yield return new WaitForSeconds(1.1f);
        //audio
        PlayLevelWonAudio();
        //disable player rigid body
        DisableInput();
        //show message on screen
        levelWonObject.SetActive(true);
        levelWonMessage.text = "Level 1 Completed!\r\nLevel 2 starting in 5...";
        yield return new WaitForSeconds(0.5f);
        levelWonMessage.text = "Level 1 Completed!\r\nLevel 2 starting in 4...";
        yield return new WaitForSeconds(0.5f);
        levelWonMessage.text = "Level 1 Completed!\r\nLevel 2 starting in 3...";
        yield return new WaitForSeconds(0.5f);
        levelWonMessage.text = "Level 1 Completed!\r\nLevel 2 starting in 2...";
        yield return new WaitForSeconds(0.5f);
        levelWonMessage.text = "Level 1 Completed!\r\nLevel 2 starting in 1...";
        yield return new WaitForSeconds(0.5f);

        levelWonObject.SetActive(false);
        //load new scene
        sceneController.LoadLevel2(noOfKeys, noOfGems, energyLevel);

    }

    private IEnumerator GameWonCoroutine()
    {
        yield return new WaitForSeconds(1.1f);
        //audio
        PlayLevelWonAudio();
        //disable player rigid body
        DisableInput();
        //show message on screen
        gameWonObject.SetActive(true);
        gameWonMessage.text = "Congratulations!";
        yield return new WaitForSeconds(2f);

        gameWonObject.SetActive(false);
        //load new scene
        SceneManager.LoadScene(3);
    }

    public void LevelWon()
    {
        StartCoroutine(LevelWonCoroutine());
    }

    public void GameWon()
    {
        StartCoroutine(GameWonCoroutine());
    }
    public void KillSoftStarEnemies()
    {
        Destroy(softStarEnemyParent);
        PlayKillGhostAudio();
    }

    public void KillHeartEnemies()
    {
        Destroy(heartEnemyParent);
        PlayKillGhostAudio();
    }

    public void KillCubieEnemies()
    {
        Destroy(cubieEnemyParent);
        PlayKillGhostAudio();
    }

    public void SetGemStatus(string gemName)
    {
        if (gemName.Equals("SoftStar")) collectedSoftStarGem = true;
        else if (gemName.Equals("Heart")) collectedHeartGem = true;
        else if (gemName.Equals("Cuboid")) collectedCubiodGem = true;
        else if (gemName.Equals("Hexagon")) collectedHexagonGem = true;

        CheckKillGhost();
    }

    public void CheckKillGhost()
    {
        if (collectedSoftStarGem && noOfKeys == 6) KillSoftStarEnemies();
        else if (collectedHeartGem && noOfKeys == 7) KillHeartEnemies();
        else if (collectedCubiodGem && collectedHexagonGem && noOfKeys == 8) KillCubieEnemies();
    }
}
