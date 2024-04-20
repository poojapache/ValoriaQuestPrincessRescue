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

    private void Awake()
    {
        InitializeScene();
        
    }

    private void InitializeScene()
    {
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        malePlayer = GameObject.Find("Male Player");
        femalePlayer = GameObject.Find("Female Player");

        if (CharcaterSelector.character == 1)
        {
            player = femalePlayer;
            Destroy(malePlayer);
        }
        else
        {
            Destroy(femalePlayer);
            player = malePlayer;
        }
        playerController = player.GetComponent<PlayerController>();
        cameraController.target = player.transform;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PatrolAndChase ghost2 = GameObject.FindWithTag("Ghost").GetComponent<PatrolAndChase>();
            ghost2.enemy = player.transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
            QuitGame();

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }


    public void LoadLevel2()
    {
        int keys = playerController.noOfKeys;
        int gems = playerController.noOfGems;
        int energy = playerController.energyLevel;
        SceneManager.LoadScene(2);

        InitializeScene();

        playerController.noOfKeys = keys;
        playerController.noOfGems = gems;
        playerController.energyLevel = energy;

        PatrolAndChase ghost2 = GameObject.FindWithTag("Ghost").GetComponent<PatrolAndChase>();
        ghost2.enemy = player.transform;
        
    }
}
