using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MediumGolemController : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Salty_Rusty_Controller.isSalty)
            agent.SetDestination(Salty_Rusty_Controller.globalSalty.transform.position);
        else
            agent.SetDestination(Salty_Rusty_Controller.globalRusty.transform.position);
    }
}
