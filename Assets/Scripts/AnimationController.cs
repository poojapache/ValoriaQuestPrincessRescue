using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float delay = 20.0f; // Delay in seconds

    void Start()
    {
        // Disable the GameObject's Animator component initially
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Start the coroutine to activate the Animator after the delay
        StartCoroutine(ActivateAnimatorDelayed(animator));
    }

    IEnumerator ActivateAnimatorDelayed(Animator animator)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Enable the Animator component
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}