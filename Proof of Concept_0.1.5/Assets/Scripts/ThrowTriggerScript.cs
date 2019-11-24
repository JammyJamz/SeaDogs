using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowTriggerScript : MonoBehaviour
{

    private Text throwText;

    public float forceMagnitude;

    public Transform landingPosition;

    // Start is called before the first frame update
    void Start()
    {
        throwText = CanvasData.throwText.GetComponent<Text>();
        throwText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        Salty_Rusty_Controller.throwForceMagnitude = forceMagnitude;
        Salty_Rusty_Controller.endThrowPoint = landingPosition;
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty") && collider.gameObject.tag == "saltyCollider")
        {
            Salty_Rusty_Controller.inThrowTrigger = true;
            throwText.gameObject.SetActive(true);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") && collider.gameObject.tag == "rustyCollider")
        {
            Salty_Rusty_Controller.inThrowTrigger = true;
            throwText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty") && collider.gameObject.tag == "saltyCollider")
        {
            Salty_Rusty_Controller.inThrowTrigger = false;
            throwText.gameObject.SetActive(false);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") && collider.gameObject.tag == "rustyCollider")
        {
            Salty_Rusty_Controller.inThrowTrigger = false;
            throwText.gameObject.SetActive(false);
        }
    }
}
