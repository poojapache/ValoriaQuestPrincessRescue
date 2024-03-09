using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunawayGhost : MonoBehaviour


{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Transform chaser = null;
    [SerializeField] private float displacementDist = 5f;
    public GameObject keyPrefab;

    // Start is called before the first frame update

    void Start()
    {


        if (agent == null)
            if (!TryGetComponent(out agent))
                Debug.LogWarning(name + " needs a navmesh agent");


    }



    // Update is called once per frame

    void Update()

    {
        

        if (chaser == null)
            return;

        Vector3 normDir = (chaser.position - transform.position).normalized;

        normDir = Quaternion.AngleAxis(Random.Range(0, 180), Vector3.up) * normDir;
        //normDir = Quaternion.AngleAxis(45, Vector3.up) * normDir;


        MoveToPos(transform.position - (normDir * displacementDist));
        
    }


    private void OnTriggerEnter(Collider c)


    {

        //Debug.Log("collision with ghoul trigger");hg


        if (c.gameObject.CompareTag("Player"))

        {

            Debug.Log("key should appear; ghost disappears");

            Vector3 keyPosition = transform.position;


            keyPosition.y = 7f;


            Instantiate(keyPrefab, keyPosition, Quaternion.identity);

        }

    }


    private void MoveToPos(Vector3 pos)

    {


        agent.SetDestination(pos);


        agent.isStopped = false;


    }
}
