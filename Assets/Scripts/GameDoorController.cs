using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDoorController : MonoBehaviour
{

    private Animator animator;
    public int noOfKeysRequired;
    public bool isDummyDoor = false;
    public GameObject keyInfoTextObject;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player")&& c.GetComponent<PlayerController>().noOfKeys == noOfKeysRequired)
        {
            keyInfoTextObject.SetActive(false);
            animator.SetBool("isOpened", true);
            c.GetComponent<PlayerController>().ClearGameInfoText();
            if (!isDummyDoor) {
                if(noOfKeysRequired == 3 && c.gameObject.GetComponent<PlayerController>().energyLevel < 60)
                {
                    c.GetComponent<PlayerController>().SetEnergyWarningText(60);
                }
                else if (noOfKeysRequired == 6 && (c.gameObject.GetComponent<PlayerController>().energyLevel < 100 || c.gameObject.GetComponent<PlayerController>().noOfGems < 4))
                {
                    c.GetComponent<PlayerController>().SetEnergyWarningText(100);

                }
                else if (noOfKeysRequired == 7)
                {
                    c.GetComponent<PlayerController>().GameWon();
                }
                
            }
            else
            {
                c.GetComponent<PlayerController>().GameLost();

            }
        }

    }
}
