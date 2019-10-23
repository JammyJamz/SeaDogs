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
    public float hangTime = 10f;

    public float angularSpeed;

    public Transform target;
    public Transform guide;

    private float distance;
    private float x, y, z;
    private float counter;

    private bool attack = false;
    private bool playerClose = false;
    private bool forceApplied = false;

    private Vector3 startPos;
    Vector3 direction;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);
        Vector3 targetDir = guide.position - transform.position;


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

    IEnumerator lookAt()
    {
        
        if (playerClose == true)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            yield return new WaitForSecondsRealtime(delayAttack);
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
        Destroy(this.gameObject);
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
