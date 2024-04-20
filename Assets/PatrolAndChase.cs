using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAndChase : MonoBehaviour
{
    public GameObject[] waypoints;
    public UnityEngine.AI.NavMeshAgent agent;
    public AIState aiState;
    private int currStaticWaypoint = 0;
    public float attackDistance = 20f; // Distance at which the agent starts fleeing from the enemy
    //public GameObject keyPrefab;
    public PlayerController playerController;
    public bool hitWall = false;
    public Transform enemy; // Reference to the enemy GameObject

    public enum AIState
    {
        staticWaddling,
        chase
    }


    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemy = playerController.gameObject.transform;
        agent.speed = 10;
        agent.acceleration = 8;
        agent.angularSpeed = 120;
        setState(AIState.staticWaddling);
    }

    void Update()
    {
        switch (aiState)
        {
            case AIState.staticWaddling:
                updateToStaticWayPoint();
                break;
            case AIState.chase:
                updatePlayerChase();
                break;
        }
    }

    private void setState(AIState newState)
    {
        aiState = newState;
        switch (aiState)
        {
            case AIState.staticWaddling:
                print("STATIC STATE");
                updateToStaticWayPoint();
                break;
            case AIState.chase:
                print("CHASE STATE");
                updatePlayerChase();
                break;
        }
    }

    void updatePlayerChase()
    {
        if (enemy != null && Vector3.Distance(transform.position, enemy.position) < attackDistance)
        {
            //Vector3 attackDirection = (enemy.position - transform.position).normalized;
            //Vector3 attackDestination = transform.position + (attackDirection * 20f);

            // Set the destination to the flee destination
            agent.SetDestination(enemy.position);
        }
        else
        {
            currStaticWaypoint = 0;
            print("gOING BACK TO STATIC");
            setState(AIState.staticWaddling);
            return;
        }
    }

    void updateToStaticWayPoint()
    {
        if (Vector3.Distance(transform.position, enemy.position) < attackDistance)
        {
            setState(AIState.chase);
        }

        if (waypoints.Length > 0)
        {
            SetDestination();
        }
        // If the agent has reached the current waypoint, move to the next one
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            //Debug.Log("Reached waypoint " + currStaticWaypoint);
            currStaticWaypoint = (currStaticWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currStaticWaypoint].transform.position);
        }
    }

    void SetDestination()
    {
        // Set the destination of the NavMeshAgent to the current waypoint
        // Debug.Log("Moving to waypoint " + currStaticWaypoint);
        agent.SetDestination(waypoints[currStaticWaypoint].transform.position);
    }

}
