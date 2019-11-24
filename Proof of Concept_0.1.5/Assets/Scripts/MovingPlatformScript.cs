using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;

    public float moveSpeed;

    private Transform salty;
    private Transform rusty;

    private bool reachedPos1;
    private bool reachedPos2;
    private float moveInc;
    private bool saltyInTrigger;
    private bool rustyInTrigger;




    // Start is called before the first frame update
    void Start()
    {
        salty = Salty_Rusty_Controller.globalSalty;
        rusty = Salty_Rusty_Controller.globalRusty;
        moveSpeed = moveSpeed / 50;
        reachedPos1 = false;
        reachedPos2 = false;
        moveInc = Mathf.Lerp(0f, Vector3.Distance(pos1.position, pos2.position), moveSpeed);
        saltyInTrigger = false;
        rustyInTrigger = false;
    }

    void Awake()
    {
        reachedPos1 = false;
        reachedPos2 = false;
        moveInc = Mathf.Lerp(0f, Vector3.Distance(pos1.position, pos2.position), moveSpeed);
        saltyInTrigger = false;
        rustyInTrigger = false;
    }

    



    private void FixedUpdate()
    {
        if(!reachedPos1 && !reachedPos2)
        {
            //transform.position = transform.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.velocity = (pos1.position - pos2.position).normalized * moveSpeed ;

            if (rustyInTrigger)
            {
                //rusty.position = rusty.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

                if (!Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody rustyRig = rusty.GetComponent<Rigidbody>();
                    rustyRig.velocity += (pos1.position - pos2.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = rusty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos1.position - pos2.position).normalized * moveSpeed;
                }
            }
            if(saltyInTrigger)
            {
                //salty.position = salty.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

                if(Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody saltyRig = salty.GetComponent<Rigidbody>();
                    saltyRig.velocity += (pos1.position - pos2.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = salty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos1.position - pos2.position).normalized * moveSpeed;
                }
                
            }

            if (Vector3.Distance(transform.position, pos1.position) < 0.1f)
                reachedPos1 = true;
        }
        else if(!reachedPos1)
        {
            //transform.position = transform.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.velocity = (pos1.position - pos2.position).normalized * moveSpeed ;

            if (saltyInTrigger)
            {
                //salty.position = salty.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

                if (Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody saltyRig = salty.GetComponent<Rigidbody>();
                    saltyRig.velocity += (pos1.position - pos2.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = salty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos1.position - pos2.position).normalized * moveSpeed;
                }
            }
            if (rustyInTrigger)
            {
                //rusty.position = rusty.position + (pos1.position - pos2.position).normalized * moveInc * Time.deltaTime;

                if (!Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody rustyRig = rusty.GetComponent<Rigidbody>();
                    rustyRig.velocity += (pos1.position - pos2.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = rusty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos1.position - pos2.position).normalized * moveSpeed;
                }
            }
            if (Vector3.Distance(transform.position, pos1.position) < 0.1f)
            {
                reachedPos1 = true;
                reachedPos2 = false;
            }
        }
        else if (!reachedPos2)
        {
            //transform.position = transform.position + (pos2.position - pos1.position).normalized * moveInc * Time.deltaTime;
            Rigidbody rb = GetComponent<Rigidbody>();

            rb.velocity = (pos2.position - pos1.position).normalized * moveSpeed ;


            if (saltyInTrigger)
            {
                //salty.position = salty.position + (pos2.position - pos1.position).normalized * moveInc * Time.deltaTime;

                if(Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody saltyRig = salty.GetComponent<Rigidbody>();
                    saltyRig.velocity += (pos2.position - pos1.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = salty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos2.position - pos1.position).normalized * moveSpeed;
                }
                
            }
            if (rustyInTrigger)
            {
                //rusty.position = rusty.position + (pos2.position - pos1.position).normalized * moveInc * Time.deltaTime;

                if (!Salty_Rusty_Controller.isSalty || true)
                {
                    Rigidbody rustyRig = rusty.GetComponent<Rigidbody>();
                    rustyRig.velocity += (pos2.position - pos1.position).normalized * moveSpeed;
                }
                else
                {
                    NavMeshAgent agent = rusty.GetComponent<NavMeshAgent>();
                    agent.velocity = (pos2.position - pos1.position).normalized * moveSpeed;
                }
            }
            if (Vector3.Distance(transform.position, pos2.position) < 0.1f)
            {
                reachedPos2 = true;
                reachedPos1 = false;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            Salty_Rusty_Controller.saltyOnPlatform = false;
            saltyInTrigger = false;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.rustyOnPlatform = false;
            rustyInTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            saltyInTrigger = true;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            rustyInTrigger = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            saltyInTrigger = true;
            Salty_Rusty_Controller.saltyOnPlatform = true;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.rustyOnPlatform = true;
            rustyInTrigger = true;
        }
    }
}
