using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public AudioSource wings;
    public float patrolSpeed;
    public float maxDiveBombSpeed;
    public float returnToPatrolSpeed;
    public float cooldownTime;
    public Transform[] patrolPoints;
    private int targetPoint;
    private Rigidbody rb;
    private bool reachedPoint;
    private bool reachedPointAfterAttack;
    public FieldOfView fov;

    private float timer;

    public bool onCooldown;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
        reachedPoint = false;
        rb = GetComponent<Rigidbody>();
        fov = GetComponent<FieldOfView>();
        onCooldown = false;
        timer = 0f;
        reachedPointAfterAttack = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(onCooldown)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= cooldownTime)
            {
                onCooldown = false;
                timer = 0f;
            }
        }

        if(Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.3f)
        {
            reachedPointAfterAttack = true;
            reachedPoint = true;
            float r = Random.value;
            if (r == 1f)
                r = 0.9f;
            int newPoint = (int)(r * patrolPoints.Length);

            targetPoint = newPoint;
        }

        Vector3 vel = (patrolPoints[targetPoint].position - transform.position).normalized * patrolSpeed;

        if(!reachedPointAfterAttack)
        {
            vel = (patrolPoints[targetPoint].position - transform.position).normalized * returnToPatrolSpeed;
        }

        fov.FindVisibleTarget();
        Vector3 saltyChestPos = Salty_Rusty_Controller.globalSalty.position + Vector3.up*0.6f;
        Vector3 rustyChestPos = Salty_Rusty_Controller.globalRusty.position + Vector3.up*0.7f;

        if(Salty_Rusty_Controller.isSalty && fov.saltyInView && !onCooldown)
        {
            if (!wings.isPlaying)
                wings.Stop();

            Vector3 dir = (saltyChestPos - transform.position).normalized;

            if(rb.velocity.magnitude <= maxDiveBombSpeed)
            {
                rb.AddForce(dir*10, ForceMode.Force);
            }
            else
            {
                rb.velocity = dir * maxDiveBombSpeed;
            }
            if(Vector3.Distance(transform.position, saltyChestPos) < 0.2f)
            {
                onCooldown = true;
                reachedPointAfterAttack = false;
            }
        }
        else if (!Salty_Rusty_Controller.isSalty && fov.rustyInView && !onCooldown)
        {
            if(!wings.isPlaying)
                wings.Stop();

            Vector3 dir = (rustyChestPos - transform.position).normalized;

            if (rb.velocity.magnitude <= maxDiveBombSpeed)
            {
                rb.AddForce(dir * 10, ForceMode.Force);
            }
            else
            {
                rb.velocity = dir * maxDiveBombSpeed;
            }
            if (Vector3.Distance(transform.position, rustyChestPos) < 0.2f)
            {
                onCooldown = true;
                reachedPointAfterAttack = false;
            }
        }
        else
        {
            if(wings.isPlaying)
                wings.Play();
            rb.velocity = vel;
        }



        

        Quaternion temp = transform.rotation;

        if (Salty_Rusty_Controller.isSalty && !onCooldown && fov.saltyInView)
            transform.LookAt(saltyChestPos);
        else if (!Salty_Rusty_Controller.isSalty && !onCooldown && fov.rustyInView)
            transform.LookAt(rustyChestPos);
        else
            transform.LookAt(patrolPoints[targetPoint]);
        transform.rotation = Quaternion.Lerp(temp, transform.rotation, 0.09f);
        
    }

}
