using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public bool isLadderButton;

    private Text buttonText;

    public GameObject ladder;

    public static bool isSalty;

    private bool ladderDropped;

    private bool inTrigger;

    private bool buttonKeyDown;
    private bool xboxButtonKeyDown;

    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        buttonKeyDown = false;
        xboxButtonKeyDown = false;
        ladderDropped = false;

        if(isLadderButton)
        {
            buttonText = CanvasData.ladderDropText.GetComponent<Text>();
        }
        else
        {
            buttonText = CanvasData.platformSpawnText.GetComponent<Text>();
        }
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

        if(!buttonKeyDown && Input.GetButtonDown("Xbox Interact"))
        {
            xboxButtonKeyDown = true;
        }
    }

    private void FixedUpdate()
    {
        if (buttonKeyDown || xboxButtonKeyDown)
        {
            if (inTrigger)
            {
                ladderDropped = true;
                ladder.SetActive(true);
            }

            
        }
        buttonKeyDown = false;
        xboxButtonKeyDown = false;
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
