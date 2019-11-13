using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPositionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.meleeTargetPosition = transform.parent.position;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") && this.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("setting true...");
            Salty_Rusty_Controller.targetInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.targetInRange = false;
            Salty_Rusty_Controller.meleeTargetPosition = Vector3.zero;
        }
    }
}
