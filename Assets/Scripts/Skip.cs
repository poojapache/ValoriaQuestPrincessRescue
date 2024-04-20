using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Skip : MonoBehaviour
{
    public GameObject script1Object;
    // Start is called before the first frame update
    public void DoSkip()
    {
        StartCameraController script1 = script1Object.GetComponent<StartCameraController>();

        script1.DoSkip();
    }
}
