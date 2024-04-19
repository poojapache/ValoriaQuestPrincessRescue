using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Root motion required components
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerController))]

public class RootMotion : MonoBehaviour
{
    private Animator ani;
    private Rigidbody rb;
    private PlayerMovementController pCtrl;

    public float animationSpeed = 1f;
    public float rootMoveSpeed = 1f;
    public float rootTurnSpeed = 1f;
    float forwardMove;
    float turnMove;
    private bool doAttack;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        pCtrl = GetComponent<PlayerMovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        forwardMove = pCtrl.Forward;
        turnMove = pCtrl.Turn;
        doAttack = pCtrl.doAttack;
    }

    private void FixedUpdate()
    {
        ani.SetFloat("velx", turnMove);
        ani.SetFloat("vely", forwardMove);
        ani.SetBool("doAttack", doAttack);
        if (doAttack)
        {
            Debug.Log("set anim attacked");
            pCtrl.doAttack = false;

        }
        ani.speed = animationSpeed;
    }

    void OnAnimatorMove()
    {
        Vector3 newRootPosition = ani.rootPosition;
        Quaternion newRootRotation = ani.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        this.transform.position = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMoveSpeed);
        this.transform.rotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, rootTurnSpeed);

        rb.MovePosition(newRootPosition);
        rb.MoveRotation(newRootRotation);
    }
}
