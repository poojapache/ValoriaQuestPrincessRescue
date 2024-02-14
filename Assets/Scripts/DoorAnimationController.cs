using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool playerLeft = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void AnimateIdle()
    {
        Animate("isIdle");
    }

    public void AnimateOpened()
    {
        Animate("isOpened");
    }
    public void AnimateClosed()
    {
        Animate("isClosed");
    }

    private void Animate(string boolName)
    {
        DisableOtherAnimations(animator, boolName);
        animator.SetBool(boolName, true);
    }

    private void DisableOtherAnimations(Animator animator, string animation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider c)
    {
        //check if player has collided, and check if player has collected the required keys
        if (c.gameObject.CompareTag("Player") && c.GetComponent<PlayerController>().noOfKeys == 1)
        {
            print("opening door");
            AnimateOpened();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && c.GetComponent<PlayerController>().noOfKeys == 1)
        {
            print("closing door");
            AnimateClosed();
            
        }
    }
}
