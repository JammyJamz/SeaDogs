using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentTracking : MonoBehaviour
{
    public float speed = 3.0f;
    public float fieldOfView;
    float distance;

    private GameObject playerTarget;
    private Transform target;

    Vector3 lastKnownPosition = Vector3.zero;
    Quaternion lookRotation;



    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectsWithTag("Player")[0];
        target = playerTarget.transform;
    }


    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(target.position, transform.position);

        if (distance < fieldOfView)
        {
            if (playerTarget)
            {
                if (lastKnownPosition != playerTarget.transform.position)
                {
                    lastKnownPosition = playerTarget.transform.position;
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

        playerTarget = target;

        return true;
    }
}
