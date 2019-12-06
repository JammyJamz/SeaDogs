using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemMeleeDetection : MonoBehaviour
{
    public SmallGolemEvents sge;

    public void OnTriggerEnter(Collider other)
    {
        if(sge.inAttackWindow && !sge.hitInThisWindow)
        {
            if (Salty_Rusty_Controller.isSalty)
            {
                if (other.gameObject.tag == "saltyCollider")
                {
                    HealthBarController.RemoveHealth();
                    sge.hitInThisWindow = true;
                }
            }
            else
            {
                if (other.gameObject.tag == "rustyCollider")
                {
                    HealthBarController.RemoveHealth();
                    sge.hitInThisWindow = true;
                }
            }
        }
    }
}
