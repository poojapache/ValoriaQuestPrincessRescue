using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCameraController : MonoBehaviour
{
    public GameObject crown;
    public GameObject congratulationsPanel;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        crown.SetActive(false);
        congratulationsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // Get the camera's current position
        var tCameraPosn = transform.localPosition;
        timer += Time.deltaTime;
        if (tCameraPosn.z <= 18.0f)
        {
            // Move the camera forward
            transform.Translate((Vector3.forward * (Time.deltaTime * 1.5f)));
        }
        else
        {
            crown.SetActive(true);
            if (timer >= 18.5)
            {
                congratulationsPanel.SetActive(true);
            }

        }

    }
}
