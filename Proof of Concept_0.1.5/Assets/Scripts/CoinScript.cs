using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public GameObject rootObj;
    private Rigidbody rb;
    private bool started;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        started = true;
        timer = 0f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0f;
        started = true;
        //rb.AddForce((Vector3.up - (new Vector3(randX, 0, randZ)).normalized) * 100f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(started && timer >= 0.05f)
        {
            float randX = Random.Range(0, 5f);
            float randZ = Random.Range(0, 5f);
            rb.AddForce((Vector3.up + (new Vector3(randX, 0, randZ)).normalized).normalized * 2f, ForceMode.Impulse);
            started = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            PauseMenu.coinsCollected++;
            Destroy(rootObj);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            PauseMenu.coinsCollected++;
            Destroy(rootObj);
        }
    }
    
}
