using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlatformOrTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            Salty_Rusty_Controller.saltyOnPlatform = false;
            //saltyInTrigger = false;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.rustyOnPlatform = false;
            //rustyInTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            //saltyInTrigger = true;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            //rustyInTrigger = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            //saltyInTrigger = true;
            Salty_Rusty_Controller.saltyOnPlatform = true;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.rustyOnPlatform = true;
            //rustyInTrigger = true;
        }
    }
}
