using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumGolemProjectile : MonoBehaviour
{
    private bool awake;

    private Rigidbody rb;

    private float timer;

    Vector3 startPos;

    public float h = 25f;
    public float throwForce = 25f;

    // Start is called before the first frame update
    void Start()
    {
        awake = true;

        timer = 0f;
        startPos = transform.position;

        if(Salty_Rusty_Controller.isSalty)
        {
            h = Mathf.Log((Salty_Rusty_Controller.globalSalty.transform.position + Vector3.up - startPos).magnitude)*0.9f;
        }
        else
        {
            h = Mathf.Log((Salty_Rusty_Controller.globalRusty.transform.position + Vector3.up - startPos).magnitude*0.9f);
        }
        
    }

    private void Awake()
    {
        timer = 0f;
        awake = true;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (awake)
        {
            //rb.velocity = CalcVelocityVector(startPos, Salty_Rusty_Controller.globalSalty.transform.position, 75);
            rb.useGravity = true;
            rb.velocity = CalculateLaunchVelocity();
            awake = false;
        }
        
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY;
        Vector3 displacementXZ;

        if (Salty_Rusty_Controller.isSalty)
        {
            displacementY = Salty_Rusty_Controller.globalSalty.transform.position.y + 1 - startPos.y;
            displacementXZ = new Vector3(Salty_Rusty_Controller.globalSalty.transform.position.x - startPos.x, 0, Salty_Rusty_Controller.globalSalty.transform.position.z - startPos.z);
        }
        else
        {
            displacementY = Salty_Rusty_Controller.globalRusty.transform.position.y + 1 - startPos.y;
            displacementXZ = new Vector3(Salty_Rusty_Controller.globalRusty.transform.position.x - startPos.x, 0, Salty_Rusty_Controller.globalRusty.transform.position.z - startPos.z);
        }

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - h) / Physics.gravity.y));



        if(float.IsNaN(velocityXZ.x) || float.IsNaN(velocityXZ.z))
        {
            if (Salty_Rusty_Controller.isSalty)
            {
                Vector3 vec;
                vec = ((Salty_Rusty_Controller.globalSalty.position - startPos).normalized + Vector3.up*0.65f).normalized*10;
                return vec;
            }
            else
            {
                Vector3 vec;
                vec = ((Salty_Rusty_Controller.globalRusty.position - startPos).normalized + Vector3.up * 0.65f).normalized * 10;
                return vec;
            }
        }
        return velocityY + velocityXZ;
        
    }
}
