using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [HideInInspector] public GameObject malePlayer;
    [HideInInspector] public GameObject femalePlayer;
    [HideInInspector] public GameObject player;
    [HideInInspector] private CameraController cameraController;
    PlayerController playerController;

    public int keys, gems, energy;

    private void Awake()
    {
        InitializeScene();
        
    }

    private void InitializeScene()
    {
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        malePlayer = GameObject.Find("Male Player");
        femalePlayer = GameObject.Find("Female Player");

        if (CharcaterSelector.character == 2)
        {
            player = femalePlayer;
            Destroy(malePlayer);
        }
        else
        {
            player = malePlayer;
            Destroy(femalePlayer);
        }
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on player GameObject.");
            return;
        }
        cameraController.target = player.transform;
    }

    public void LoadLevel2(int keys, int gems, int energy)
    {
        this.keys = keys;
        this.energy = energy;
        this.gems = gems;

        SceneManager.LoadScene(2);

        InitializeScene();

    }


}
