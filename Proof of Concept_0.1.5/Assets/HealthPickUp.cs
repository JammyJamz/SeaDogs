using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            if(HealthBarController.currentHealth < 3)
            {
                HealthBarController.AddHealth();
                GameObject.Destroy(gameObject);
            }
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            if (HealthBarController.currentHealth < 3)
            {
                HealthBarController.AddHealth();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
