using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryWinScript : MonoBehaviour
{
    public static bool gameWon;
    // Start is called before the first frame update
    void Start()
    {
        gameWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if (other.gameObject.tag == "saltyCollider")
            {
                int jungleKey = PlayerPrefs.GetInt("jungleKey", 0);
                int templeKey = PlayerPrefs.GetInt("templeKey", 0);
                if (jungleKey == 1 && templeKey == 1)
                {
                    gameWon = true;
                    PauseMenu.gameWon = true;
                }
            }
        }
        else
        {
            if (other.gameObject.tag == "rustyCollider")
            {
                int jungleKey = PlayerPrefs.GetInt("jungleKey", 0);
                int templeKey = PlayerPrefs.GetInt("templeKey", 0);
                if (jungleKey == 1 && templeKey == 1)
                {
                    gameWon = true;
                    PauseMenu.GameWon();
                }
            }
        }
    }
}
