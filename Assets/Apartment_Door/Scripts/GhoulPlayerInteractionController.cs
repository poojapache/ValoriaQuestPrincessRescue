using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GhoulPlayerInteractionController : MonoBehaviour
{
    public bool flag = true;
    public GameObject gameOverGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        //Debug.Log("collision with ghoul trigger");
        if (c.gameObject.CompareTag("Player") && flag)
        {
            Debug.Log("game ended");
            Time.timeScale = 0f;
            gameOverGameObject.SetActive(true);
            flag = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
