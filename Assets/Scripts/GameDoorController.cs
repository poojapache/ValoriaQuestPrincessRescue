using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDoorController : MonoBehaviour
{

    private Animator animator;
    public int noOfKeysRequired;
    public bool isDummyDoor = false;
    public GameObject keyInfoTextObject;
    private PlayerController playerController;
    public Transform respawnTransform;
    private GameObject player;
    private GameObject screenRespawnMessage;
    private TextMeshProUGUI respawnMessage;
    private GameObject keyInfo;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        //respawn message is the child of scree
        screenRespawnMessage = playerController.respawnObject;
        respawnMessage = screenRespawnMessage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        keyInfo = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider c)
    {
        //chek if collider belongs to player and check if player has enough keys
        if (c.gameObject.CompareTag("Player") && c.GetComponent<PlayerController>().noOfKeys == noOfKeysRequired)
        {
            keyInfoTextObject.SetActive(false);
            //open door
            playerController.DoorOpen(animator);
            //remove key required text on screen
            playerController.ClearGameInfoText();
            if (!isDummyDoor)
            {
                //ghost 1 room
                if (noOfKeysRequired == 3 && playerController.energyLevel < playerController.ghost1Energy)
                {
                    playerController.SetEnergyWarningText(1);
                }
                //ghost 2 room
                else if (noOfKeysRequired == 6 && (playerController.energyLevel < playerController.ghost2Energy || playerController.noOfGems < playerController.ghost2Gems))
                {
                    playerController.SetEnergyWarningText(2);

                }
                //last door
                else if (noOfKeysRequired == 7)
                {
                    StartCoroutine(GameWonCoroutine());
                }

            }
            else
            {
                //spawn in previous room
                StartCoroutine(RespawnControllerCoroutine());
            }
        }

    }

    private IEnumerator RespawnControllerCoroutine()
    {
        yield return new WaitForSeconds(2.1f);
        //disable player controller
        playerController.enabled = false;
        //show message on screen
        screenRespawnMessage.SetActive(true);
        respawnMessage.text = "Oops, you opened a dummy door!\r\nYou will be respawned to another room in 5... ";
        yield return new WaitForSeconds(2f);
        respawnMessage.text = "Oops, you opened a dummy door!\r\nYou will be respawned to another room in 4... ";
        yield return new WaitForSeconds(2f);
        respawnMessage.text = "Oops, you opened a dummy door!\r\nYou will be respawned to another room in 3...";
        yield return new WaitForSeconds(2f);
        respawnMessage.text = "Oops, you opened a dummy door!\r\nYou will be respawned to another room in 2...";
        yield return new WaitForSeconds(2f);
        respawnMessage.text = "Oops, you opened a dummy door!\r\nYou will be respawned to another room in 1...";
        yield return new WaitForSeconds(2f);
        //close door
        animator.SetInteger("doorVal", 3);
        keyInfo.SetActive(true);
        //adjust lighting
        // respawn
        player.transform.position = respawnTransform.position;
        player.transform.rotation = respawnTransform.rotation;
        //resume game
        playerController.enabled = true;
        //hide respawn message
        screenRespawnMessage.SetActive(false);
    }

    private IEnumerator GameWonCoroutine()
    {
        yield return new WaitForSeconds(3f);
        playerController.GameWon();

    }
}
