using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHitDetection : MonoBehaviour
{
    public BatController bc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if (!bc.onCooldown && other.gameObject.tag == "saltyCollider")
            {
                HealthBarController.RemoveHealth();
            }
        }
        else if (!Salty_Rusty_Controller.isSalty)
        {
            if (!bc.onCooldown && other.gameObject.tag == "rustyCollider")
            {
                HealthBarController.RemoveHealth();
            }
        }
    }
}
