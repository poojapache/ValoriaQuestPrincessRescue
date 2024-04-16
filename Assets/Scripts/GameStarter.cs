using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public GameObject currentPanel; //The current Instructions Panel
    public GameObject previousPanel; //The Game Menu Panel
    public GameObject nextPanel; //The Game Menu Panel

    public void Start()
    {
        currentPanel.SetActive(true);
        previousPanel.SetActive(false);
    }
    // Start is called before the first frame update
    public void ContinueGame()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
    public void BackGame()
    {
        currentPanel.SetActive(false);
        previousPanel.SetActive(true);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
