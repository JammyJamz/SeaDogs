using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    public Text buttonText;

    public GameObject ladder;

    public static bool isSalty;

    private bool ladderDropped;

    private bool inTrigger;

    private bool buttonKeyDown;

    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        buttonKeyDown = false;
        ladderDropped = false;
    }

    void Awake()
    {
        inTrigger = false;
        buttonKeyDown = false;
        ladderDropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSalty)
        {
            buttonText.gameObject.SetActive(false);
        }
        if(Input.GetKeyDown("3"))
            buttonKeyDown = true;
    }

    private void FixedUpdate()
    {
        if (buttonKeyDown)
        {
            if (inTrigger)
            {
                ladderDropped = true;
                ladder.SetActive(true);
            }

            buttonKeyDown = false;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!ladderDropped && collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            buttonText.gameObject.SetActive(true);
            inTrigger = true;
        }
        else if(ladderDropped)
        {
            //Debug.Log(0);
            buttonText.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            buttonText.gameObject.SetActive(false);
            inTrigger = false;
        }
    }
}
