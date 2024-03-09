using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClickStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageToShowHide; // Reference to the image to show/hide
    public GameObject currentPanel; //The current Game Menu Panel
    public GameObject nextPanel; //The instructions Game Menu Panel

    void Start()
    {
        // Hide the image initially
        imageToShowHide.gameObject.SetActive(false);
        currentPanel.SetActive(true);
        nextPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
            QuitGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the image on hover
        imageToShowHide.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the image when not hovered
        imageToShowHide.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    public void StartGame()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}
