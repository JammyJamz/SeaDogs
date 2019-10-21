using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public Transform pressedHeight;

    private bool inTrigger;

    private Vector3 unpressedHeight;

    public Animator doorAnimator;


    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        unpressedHeight = transform.position;
    }

    void Awake()
    {
        inTrigger = false;
        unpressedHeight = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(inTrigger)
        {
            transform.position = Vector3.Lerp(transform.position, pressedHeight.position, 0.009f);
            doorAnimator.SetBool("doorOpened", true);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, unpressedHeight, 0.009f);
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("PushableBlocks"))
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("PushableBlocks"))
        {
            inTrigger = false;
        }
    }
}
