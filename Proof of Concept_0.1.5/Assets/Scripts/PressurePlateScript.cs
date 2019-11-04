using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public Transform pressedHeight;

    private bool inTrigger;

    private Vector3 unpressedHeight;

    public Animator doorAnimator;

    private bool _isLerping;
    private float _timeStartedLerping;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float timeTakenDuringLerp = 0.5f;

    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        if(inTrigger)
        {
            _startPosition = transform.position;
            _endPosition = pressedHeight.position;
        }
        else
        {
            _startPosition = transform.position;
            _endPosition = unpressedHeight;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        unpressedHeight = transform.position;
        _isLerping = false;
    }

    void Awake()
    {
        inTrigger = false;
        unpressedHeight = transform.position;
        _isLerping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);


            if (transform.position == _endPosition)
            {
                if(inTrigger)
                {
                    doorAnimator.SetBool("doorOpened", true);
                }
                _isLerping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("PushableBlocks"))
        {
            inTrigger = true;
            StartLerping();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("PushableBlocks"))
        {
            inTrigger = false;
            StartLerping();
        }
    }
}
