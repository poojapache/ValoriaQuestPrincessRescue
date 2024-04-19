using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolandRunaway : MonoBehaviour
{
    public GameObject[] waypoints;
    public UnityEngine.AI.NavMeshAgent agent;
    private bool movingBool = false;
    public AIState aiState;
    private int currStaticWaypoint = 0;
    public float fleeDistance = 30f; // Distance at which the agent starts fleeing from the enemy
    [SerializeField] private float displacementDist = 20f;
    //public GameObject keyPrefab;
    public bool hitWall = false;
    private int energyLevel;
    private GameObject enemyGameObject;
    public Transform enemy; 

    public enum AIState
    {
        staticWaddling,
        runaway
    }


    void Start()
    {
        enemyGameObject = GameObject.FindWithTag("SceneController").GetComponent<SceneController>().player;
        enemy = enemyGameObject.transform;
        energyLevel = enemy.GetComponent<PlayerController>().ghost2Energy;
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
            case AIState.runaway:
                updatePlayerRunaway();
                break;
        }
    }

    private void setState(AIState newState)
    {
        aiState = newState;
        switch (aiState)
        {
            case AIState.staticWaddling:
                print("static");
                updateToStaticWayPoint();
                break;
            case AIState.runaway:
                print("runaway");
                updatePlayerRunaway();
                break;
        }
    }

    void updatePlayerRunaway()
    {

        if (enemy != null && Vector3.Distance(transform.position, enemy.position) < 30f)
        {
            if (hitWall == false)
            {
                Vector3 fleeDirection = (enemy.position - transform.position).normalized;
                Vector3 fleeDestination = transform.position - (fleeDirection * 15f);

                // Set the destination to the flee destination
                agent.SetDestination(fleeDestination);
            }
            else
            {
                int randomWaypointIndex = Random.Range(0, 3);
                transform.position = waypoints[randomWaypointIndex].transform.position;
                hitWall = false;
            }
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
        if (Vector3.Distance(transform.position, enemy.position) < fleeDistance)
        {
            Debug.Log("RUNAWAY RUNAWAY ");
            setState(AIState.runaway);
            return;
        }

        if (waypoints.Length > 0)
        {
            SetDestination();
        }
        else
        {
            Debug.Log("No waypoints assigned to the WaypointNavigation script attached ");
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
        agent.SetDestination(waypoints[currStaticWaypoint].transform.position);
    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider c)
    {
        //if (c.gameObject.CompareTag("Player"))
        //{
        //    // Debug.Log("key should appear; ghost disappears");
        //    Vector3 keyPosition = transform.position;
        //    keyPosition.y = 7f;
        //}
        if (c.gameObject.CompareTag("Wall"))
        {
            hitWall = true;
        }
    }
}


