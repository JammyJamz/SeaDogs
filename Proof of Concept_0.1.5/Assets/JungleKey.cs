using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleKey : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("jungleKey", -100) == -100)
        {
            gameObject.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("jungleKey", -100) == 1)
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
                PlayerPrefs.SetInt("jungleKey", 1);
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (other.gameObject.tag == "rustyCollider")
            {
                PlayerPrefs.SetInt("jungleKey", 1);
                gameObject.SetActive(false);
            }
        }
    }
}
