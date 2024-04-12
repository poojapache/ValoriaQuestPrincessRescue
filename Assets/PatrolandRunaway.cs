using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolandRunaway : MonoBehaviour
{
    public GameObject[] waypoints;
    public NavMeshAgent agent;
    private bool movingBool = false;
    public AIState aiState;
    private int currStaticWaypoint = 0;
    public Transform enemy; // Reference to the enemy GameObject
    public float fleeDistance = 8f; // Distance at which the agent starts fleeing from the enemy
    [SerializeField] private float displacementDist = 8f;
    //public GameObject keyPrefab;
    public int energyLevel;

    public enum AIState
    {
        staticWaddling,
        runaway
    }


    void Start()
    {
        agent.speed = 6;
        agent.acceleration = 3;
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
                updateToStaticWayPoint();
                break;
            case AIState.runaway:
                updatePlayerRunaway();
                break;
        }
    }

    void updatePlayerRunaway()
    {

        if (enemy != null && Vector3.Distance(transform.position, enemy.position) < fleeDistance)
        {
            /*
            // Calculate a destination that moves away from the enemy
            Vector3 fleeDirection = transform.position - enemy.position;
            Vector3 fleeDestination = transform.position + fleeDirection.normalized * fleeDistance;

            // Set the destination to the flee destination
            agent.SetDestination(fleeDestination);
            

            Vector3 normDir = (enemy.position - transform.position).normalized;

            normDir = Quaternion.AngleAxis(Random.Range(0, 180), Vector3.up) * normDir;
            //normDir = Quaternion.AngleAxis(45, Vector3.up) * normDir;

            MoveToPos(transform.position - (normDir * displacementDist));
            */
            Vector3 fleeDirection = transform.position - enemy.position;
            Vector3 fleeDestination = transform.position + fleeDirection.normalized * fleeDistance;

            // Set the destination to the flee destination
            agent.SetDestination(fleeDestination);
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
        if(Vector3.Distance(transform.position, enemy.position) < fleeDistance)
        {
            Debug.LogError("RUNAWAY RUNAWAY ");
            setState(AIState.runaway);
            return;
        }

        if (waypoints.Length > 0)
        {
            SetDestination();
        }
        else
        {
            Debug.LogError("No waypoints assigned to the WaypointNavigation script attached ");
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
        //Debug.Log("Moving to waypoint " + currStaticWaypoint);
        agent.SetDestination(waypoints[currStaticWaypoint].transform.position);
    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Debug.Log("key should appear; ghost disappears");
            Vector3 keyPosition = transform.position;
            keyPosition.y = 7f;
            //Instantiate(keyPrefab, keyPosition, Quaternion.identity);
            c.gameObject.GetComponent<PlayerController>().CollectKey();
        }
    }

}
