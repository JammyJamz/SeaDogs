using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyTest : MonoBehaviour
{
    public float speed;
    public float width;
    public float height;
    public float length;
    public float lookRadius = 10f;
    public float delayAttack = 2f;
    public float delayToStart = 5f;
    public float hangTime = 10f;

    public float angularSpeed;

    public Transform target;
    public Transform guide;

    public Transform saltyPos;
    public Transform rustyPos;

    private float distance;
    private float x, y, z;
    private float counter;

    private bool attack = false;
    private bool playerClose = false;
    private bool forceApplied = false;

    private bool hitPlayer = false;

    private Vector3 startPos;
    Vector3 direction;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Salty_Rusty_Controller.isSalty)
        {
            Debug.Log("salty");
            target = saltyPos;
        }
        else
        {
            target = rustyPos;
        }
    

    // Update is called once per frame
    
        distance = Vector3.Distance(target.position, transform.position);
        Vector3 targetDir = guide.position - transform.position;

        if(!hitPlayer)
        {
            if (attack == false)
            {
                counter += Time.deltaTime;
                x = Mathf.Cos(counter) * width;
                y = Mathf.Sin(counter) * height;
                z = Mathf.Sin(counter) * length;

                // Newly added for rotation along its path.
                // Vector3 futurePos = new Vector3(x,y,z);
                // transform.LookAt(futurePos);

                float step = angularSpeed * Time.deltaTime;

                transform.position = startPos + new Vector3(x, y, z);

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

            }

            if (distance <= lookRadius)
            {
                attack = true;
                if (!forceApplied)
                {
                    playerClose = true;
                }
                // Debug.Log(attack);
                StartCoroutine("lookAt");
            }
        }
        else
        {
            StartCoroutine("GoToStartPosition");
            
        }
    }

    IEnumerator GoToStartPosition()
    {
        yield return new WaitForSeconds(delayToStart);
        transform.position = Vector3.Lerp(transform.position, startPos, 0.09f);
        if(Vector3.Distance(startPos, transform.position) <= 0.1f)
        {
            transform.position = startPos;
            hitPlayer = false;
        }

    }

    IEnumerator lookAt()
    {
        
        if (playerClose == true)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            yield return new WaitForSeconds(delayAttack);
        }

        // transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if (!forceApplied)
        {
            forceApplied = true;
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
        playerClose = false;

        Tidy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(this.gameObject);
        hitPlayer = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void Tidy()
    {
        Destroy(gameObject, hangTime);
    }
}
