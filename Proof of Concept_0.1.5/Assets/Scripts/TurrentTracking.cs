using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentTracking : MonoBehaviour
{
    public float speed = 3.0f;
    public float fieldOfView;
    float distance;

    //private GameObject playerTarget;
    public Transform saltyPos;
    public Transform rustyPos;

    private Transform target;

    Vector3 lastKnownPosition = Vector3.zero;
    Quaternion lookRotation;



    private void Awake()
    {
        //playerTarget = GameObject.FindGameObjectsWithTag("Player")[0];
      // target = playerTarget.transform;
    }


    // Update is called once per frame
    void Update()
    {

        if (Salty_Rusty_Controller.isSalty)
            target = saltyPos;

        else
            target = rustyPos;

        distance = Vector3.Distance(target.position, transform.position);

        if (distance < fieldOfView)
        {
            if (target)
            {
                if (lastKnownPosition != target.transform.position)
                {
                    lastKnownPosition = target.transform.position;
                    lookRotation = Quaternion.LookRotation(lastKnownPosition - transform.position);
                }

                if (transform.rotation != lookRotation)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);

            }
        }
    }

    bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }


        return true;
    }
}
