using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;

    public Transform salty;
    public Transform rusty;

    private bool reachedPos1;
    private bool reachedPos2;
    private float moveInc;
    private bool saltyInTrigger;
    private bool rustyInTrigger;


    // Start is called before the first frame update
    void Start()
    {
        reachedPos1 = false;
        reachedPos2 = false;
        moveInc = Mathf.Lerp(0f, Vector3.Distance(pos1.position, pos2.position), 0.0005f);
        saltyInTrigger = false;
        rustyInTrigger = false;
    }

    void Awake()
    {
        reachedPos1 = false;
        reachedPos2 = false;
        moveInc = Mathf.Lerp(0f, Vector3.Distance(pos1.position, pos2.position), 0.0005f);
        saltyInTrigger = false;
        rustyInTrigger = false;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!reachedPos1 && !reachedPos2)
        {
            transform.position = transform.position + (pos1.position - pos2.position).normalized * moveInc;

            if(rustyInTrigger)
            {
                rusty.position = rusty.position + (pos1.position - pos2.position).normalized * moveInc;
            }
            if(saltyInTrigger)
            {
                salty.position = salty.position + (pos1.position - pos2.position).normalized * moveInc;
            }

            if (Vector3.Distance(transform.position, pos1.position) < 0.1f)
                reachedPos1 = true;
        }
        else if(!reachedPos1)
        {
            transform.position = transform.position + (pos1.position - pos2.position).normalized * moveInc;

            if(saltyInTrigger)
            {
                salty.position = salty.position + (pos1.position - pos2.position).normalized * moveInc;
            }
            if (rustyInTrigger)
            {
                rusty.position = rusty.position + (pos1.position - pos2.position).normalized * moveInc;
            }
            if (Vector3.Distance(transform.position, pos1.position) < 0.1f)
            {
                reachedPos1 = true;
                reachedPos2 = false;
            }
        }
        else if (!reachedPos2)
        {
            transform.position = transform.position + (pos2.position - pos1.position).normalized * moveInc;

            if (saltyInTrigger)
            {
                salty.position = salty.position + (pos2.position - pos1.position).normalized * moveInc;
            }
            if (rustyInTrigger)
            {
                rusty.position = rusty.position + (pos2.position - pos1.position).normalized * moveInc;
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
            saltyInTrigger = false;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
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
}
