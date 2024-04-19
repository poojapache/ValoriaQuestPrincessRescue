using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharcaterSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    //public Texture2D cursorTexture;//Set it to handcursor image in Images
    public CursorMode cursorMode = CursorMode.Auto;//By default
    public Vector2 hotSpot = Vector2.zero;//By default
    public static int character = 1;//To control character selection
    public Image imageToShowHide; // Reference to the image to show/hide
    public Image imageToHide; // Reference to the image to show/hide
    private bool selected = false;

    void Start()
    {
        // Hide the image initially
        imageToShowHide.gameObject.SetActive(false);
    }

    //Sets Male character
    public void MaleCharcaterSelect()
    {
        character = 0;
    }

    //Sets Female character
    public void FemaleCharcaterSelect()
    {
        character = 1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the image on hover
        imageToShowHide.gameObject.SetActive(true);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the image when not hovered
        if (!selected)
        {
            imageToShowHide.gameObject.SetActive(false);
        }
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

    public void OnSelect(BaseEventData eventData)
    {
        // Show the image when selected
        imageToHide.gameObject.SetActive(false);
        imageToShowHide.gameObject.SetActive(true);
        selected = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}
