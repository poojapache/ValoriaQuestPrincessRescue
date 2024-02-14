using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulController : MonoBehaviour
{
    public bool flag = true;
    public Animation ghoulAnimation;
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
        if(c.gameObject.CompareTag("Player") && flag)
        {
            Debug.Log("playing ghoul anim");
            ghoulAnimation.Play();
            flag = false;
        }
    }
}
