using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MediumGolemController : MonoBehaviour
{
    public static bool slam;

    public Transform projectileStartPos;

    public GameObject projectile;

    public Animator anim;

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        slam = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if(Vector3.Distance(Salty_Rusty_Controller.globalSalty.transform.position, transform.position) < 10f)
            {
                transform.rotation = Quaternion.LookRotation(Salty_Rusty_Controller.globalSalty.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                agent.isStopped = true;
                anim.SetBool("isPatrolling", false);
                
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(Salty_Rusty_Controller.globalSalty.transform.position);
                anim.SetBool("isPatrolling", true);
            }
        }
        else
        {
            if (Vector3.Distance(Salty_Rusty_Controller.globalRusty.transform.position, transform.position) < 10f)
            {
                transform.rotation = Quaternion.LookRotation(Salty_Rusty_Controller.globalRusty.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                agent.isStopped = true;
                anim.SetBool("isPatrolling", false);

            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(Salty_Rusty_Controller.globalRusty.transform.position);
                anim.SetBool("isPatrolling", true);
            }
        }

    }

    private void FixedUpdate()
    {
        if(slam)
        {
            Instantiate(projectile, projectileStartPos.position, transform.rotation);
            slam = false;
        }
    }

}
