using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Sprite saltyImg;
    public Sprite rustyImg;
    private bool change = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = saltyImg;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && gameObject.GetComponent<Image>().sprite == saltyImg)
        {
            gameObject.GetComponent<Image>().sprite = rustyImg;
        }

        else if (Input.GetKeyDown(KeyCode.T) && gameObject.GetComponent<Image>().sprite == rustyImg)
        {
            gameObject.GetComponent<Image>().sprite = saltyImg;
        }
            

    }
}
