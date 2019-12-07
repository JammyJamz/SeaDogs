using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleKey : MonoBehaviour
{

    private void Start()
    {
        if(PlayerPrefs.GetInt("templeKey", -100) == -100)
        {
            gameObject.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("templeKey", -100) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if (other.gameObject.tag == "saltyCollider")
            {
                PlayerPrefs.SetInt("templeKey", 1);
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (other.gameObject.tag == "rustyCollider")
            {
                PlayerPrefs.SetInt("templeKey", 1);
                gameObject.SetActive(false);
            }
        }
    }
}
