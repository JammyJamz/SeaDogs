using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeTriggerScript : MonoBehaviour
{

    public Transform ledgePos;

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
        Debug.Log(collider.tag);
        if (collider.gameObject.tag == "saltyLeftHand")
        {
            Salty_Rusty_Controller.saltyLeftHandInTrigger = true;
            SaltyIKControl.ledgePosition = ledgePos.position;
            SaltyIKControl.saltyLeftHandInTrigger = true;
        }
        else if (collider.gameObject.tag == "saltyRightHand")
        {
            Salty_Rusty_Controller.saltyRightHandInTrigger = true;
            SaltyIKControl.ledgePosition = ledgePos.position;
            SaltyIKControl.saltyRightHandInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "saltyLeftHand")
        {
            Salty_Rusty_Controller.saltyLeftHandInTrigger = false;
            SaltyIKControl.saltyLeftHandInTrigger = false;
        }
        else if (collider.gameObject.tag == "saltyRightHand")
        {
            Salty_Rusty_Controller.saltyRightHandInTrigger = false;
            SaltyIKControl.saltyRightHandInTrigger = false;
        }
    }
}
