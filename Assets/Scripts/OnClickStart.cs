using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClickStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageToShowHide; // Reference to the image to show/hide

    void Start()
    {
        // Hide the image initially
        imageToShowHide.gameObject.SetActive(false);
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
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
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
