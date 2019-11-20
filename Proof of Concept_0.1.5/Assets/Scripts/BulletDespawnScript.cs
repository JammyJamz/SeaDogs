using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawnScript : MonoBehaviour
{

    private bool awake;

    private Rigidbody rb;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        awake = true;

        timer = 0f;
    }

    private void Awake()
    {
        timer = 0f;
        awake = true;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 5f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(awake)
        {
            rb.AddForce(transform.forward*50f, ForceMode.Impulse);
            awake = false;
        }
    }
}
