using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCanvasTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if (other.gameObject.tag == "saltyCollider")
            {
                int jungleKey = PlayerPrefs.GetInt("jungleKey", 0);
                int templeKey = PlayerPrefs.GetInt("templeKey", 0);
                if (jungleKey != 1)
                {
                    CanvasData.missingJungleText.SetActive(true);
                }
                else
                {
                    CanvasData.missingJungleText.SetActive(false);
                }
                if (templeKey != 1)
                {
                    CanvasData.missingTempleText.SetActive(true);
                }
                else
                {
                    CanvasData.missingTempleText.SetActive(false);
                }
            }
        }
        else
        {
            if (other.gameObject.tag == "rustyCollider")
            {
                int jungleKey = PlayerPrefs.GetInt("jungleKey", 0);
                int templeKey = PlayerPrefs.GetInt("templeKey", 0);
                if (jungleKey != 1)
                {
                    CanvasData.missingJungleText.SetActive(true);
                }
                else
                {
                    CanvasData.missingJungleText.SetActive(false);
                }
                if (templeKey != 1)
                {
                    CanvasData.missingTempleText.SetActive(true);
                }
                else
                {
                    CanvasData.missingTempleText.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            if (other.gameObject.tag == "saltyCollider")
            {
                CanvasData.missingJungleText.SetActive(false);
                CanvasData.missingTempleText.SetActive(false);
            }
        }
        else
        {
            if (other.gameObject.tag == "rustyCollider")
            {
                CanvasData.missingJungleText.SetActive(false);
                CanvasData.missingTempleText.SetActive(false);
            }
        }
    }
}
