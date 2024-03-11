using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDoorController : MonoBehaviour
{
<<<<<<< Updated upstream
    public Animator animator;
=======
    private Animator animator;
>>>>>>> Stashed changes
    public int noOfKeysRequired;
    public bool isDummyDoor = false;
    public GameObject gameOverGameObject;


    void Start()
    {
<<<<<<< Updated upstream

=======
        animator = GetComponent<Animator>();
>>>>>>> Stashed changes
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player")&& c.GetComponent<PlayerController>().noOfKeys == noOfKeysRequired)
        {
            if(!isDummyDoor) animator.SetBool("isOpened", true);
            else
            {
                animator.SetBool("isOpened", true);
                Time.timeScale = 0f;
                gameOverGameObject.SetActive(true);

            }
        }

    }
}
