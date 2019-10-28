using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallCanvasUpdate : MonoBehaviour
{
    private bool enteredTrigger;
    public Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        enteredTrigger = false;
    }

    void Awake()
    {
        enteredTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enteredTrigger)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") || collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            enteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") || collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            enteredTrigger = false;
        }
    }
}
