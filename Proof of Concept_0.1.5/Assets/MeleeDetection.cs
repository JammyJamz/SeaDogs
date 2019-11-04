using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "rustyRightHand" && Salty_Rusty_Controller.inPunchAnimation)
        {
            Salty_Rusty_Controller.targetInRange = false;
            GameObject.Destroy(transform.root.gameObject);
        }
    }
}
