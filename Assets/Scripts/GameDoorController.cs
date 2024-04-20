using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        //if (!isDummyDoor && c.gameObject.CompareTag("Player") && c.GetComponent<PlayerController>().noOfKeys == noOfKeysRequired && animator.GetInteger("doorVal") == 1)
        //{
        //    playerController = c.GetComponent<PlayerController>();

        //    //last door of level 1 -> level won
        //    if (noOfKeysRequired == 6)
        //    {
        //        if (playerController.noOfGems == 4) playerController.LevelWon();
        //        else playerController.SetGemWarningText(4);
        //    }
        //    //last door of level 2 - game won
        //    else if (noOfKeysRequired == 9)
        //    {
        //        if (playerController.noOfGems == 8) playerController.GameWon();
        //        else playerController.SetGemWarningText(8);
        //    }
        //}
        //check if collider belongs to player and check if player has enough keys
        if (c.gameObject.CompareTag("Player") && c.GetComponent<PlayerController>().noOfKeys == noOfKeysRequired && animator.GetInteger("doorVal") == 1)
        {
            playerController = c.GetComponent<PlayerController>();
            if (!isDummyDoor)
            {
                //ghost 1 room
                if (noOfKeysRequired == 3 && playerController.energyLevel < playerController.ghost1Energy)
                {
                    DoorOpenSequence();
                    playerController.SetEnergyWarningText(1);
                }
                else if (noOfKeysRequired == 6)
                {
                    if (playerController.noOfGems == 5)
                    {
                        DoorOpenSequence();
                        playerController.LevelWon();
                    }
                    else playerController.SetGemWarningText(5);
                }
                else if (noOfKeysRequired == 9)
                {
                    if (playerController.noOfGems == 9)
                    {
                        DoorOpenSequence();
                        playerController.GameWon();
                    }
                    else playerController.SetGemWarningText(9);
                }
                else DoorOpenSequence();

            }
            else
            {
                DoorOpenSequence();
                playerController.Respawn(animator, keyInfoGameObject, respawnRoomLight, respawnTransform);
            }
        }

    }
    private void DoorOpenSequence()
    {
        keyInfoGameObject.SetActive(false);
        //open door
        playerController.DoorOpen(animator);
        //remove key required text on screen
        playerController.ClearGameInfoText();
    }
}
