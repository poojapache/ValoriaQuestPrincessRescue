using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [HideInInspector] public GameObject malePlayer;
    [HideInInspector] public GameObject femalePlayer;
    [HideInInspector] public GameObject player;

    public GameObject softStarEnemyParent;

    private void Awake()
    {
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

    public void KillSoftStarEnemies()
    {
        Destroy(softStarEnemyParent);
    }
}
