using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDoorController : MonoBehaviour
{

    private Animator animator;
    public int noOfKeysRequired;
    public bool isDummyDoor = false;
    private PlayerController playerController;
    public Transform respawnTransform;
    private GameObject player;
    private GameObject keyInfoGameObject;
    public GameObject respawnRoomLight;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        keyInfoGameObject = transform.GetChild(0).gameObject;
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
            keyInfoGameObject.SetActive(false);
            //open door
            playerController.DoorOpen(animator);
            //remove key required text on screen
            playerController.ClearGameInfoText();
            if (!isDummyDoor) {
                //ghost 1 room
                if(noOfKeysRequired == 3 && playerController.energyLevel < playerController.ghost1Energy)
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
        //audio
        playerController.PlayDummyDoorAudio();
        //disable player controller
        playerController.enabled = false;
        //show message on screen
        playerController.respawnObject.SetActive(true);
        playerController.respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 5... ";
        yield return new WaitForSeconds(0.5f);
        playerController.respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 4... ";
        yield return new WaitForSeconds(0.5f);
        playerController.respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 3...";
        yield return new WaitForSeconds(0.5f);
        playerController.respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 2...";
        yield return new WaitForSeconds(0.5f);
        playerController.respawnMessage.text = "Oh no, you opened a dummy door!\r\nYou will be respawned to another room in 1...";
        yield return new WaitForSeconds(0.5f);
        //close door
        animator.SetInteger("doorVal", 3);
        keyInfoGameObject.SetActive(true);
        //adjust lighting
        respawnRoomLight.SetActive(false);
        // respawn
        player.transform.position = respawnTransform.position;
        player.transform.rotation = respawnTransform.rotation;
        //resume game
        playerController.enabled = true;
        //hide respawn message
        playerController.respawnObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        respawnRoomLight.SetActive(true);
    }

    private IEnumerator GameWonCoroutine()
    {
        yield return new WaitForSeconds(3f);
        playerController.GameWon();

    }
}
