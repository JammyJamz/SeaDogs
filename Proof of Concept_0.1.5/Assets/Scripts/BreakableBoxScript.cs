using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBoxScript : MonoBehaviour
{

    public GameObject breakableBox;

    private bool enteredTrigger;

    // Start is called before the first frame update
    void Start()
    {
        enteredTrigger = false;
    }
    void Awake()
    {
        enteredTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(enteredTrigger)
        {
            Instantiate(breakableBox, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            enteredTrigger = true;
        }
    }
}
