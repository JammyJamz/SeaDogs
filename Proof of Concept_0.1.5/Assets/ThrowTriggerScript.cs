using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowTriggerScript : MonoBehaviour
{

    public Text throwText;
    // Start is called before the first frame update
    void Start()
    {
        throwText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        throwText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            Salty_Rusty_Controller.inThrowTrigger = true;
            throwText.gameObject.SetActive(true);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.inThrowTrigger = true;
            throwText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            Salty_Rusty_Controller.inThrowTrigger = false;
            throwText.gameObject.SetActive(false);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            Salty_Rusty_Controller.inThrowTrigger = false;
            throwText.gameObject.SetActive(false);
        }
    }
}
