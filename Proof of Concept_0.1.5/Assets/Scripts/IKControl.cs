using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

    protected Animator animator;

    public float positionWeight;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;

    public Transform [] leftPoints;
    public Transform [] leftHandPoints;
    public Transform [] rightPoints;
    public Transform [] rightHandPoints;


    private bool startedAnimation;

    private int leftFootIndex;
    private int rightFootIndex;
    private int rightHandIndex;
    private int leftHandIndex;

    private bool _isLerpingLF;
    private bool _isLerpingRF;
    private bool _isLerpingRH;
    private bool _isLerpingLH;
    private float _timeStartedLerpingLF;
    private float _timeStartedLerpingRF;
    private float _timeStartedLerpingRH;
    private float _timeStartedLerpingLH;
    private Vector3 _startPositionLeftFoot;
    private Vector3 _startPositionRightFoot;
    private Vector3 _startPositionRightHand;
    private Vector3 _startPositionLeftHand;
    private Vector3 _endPositionLeftFoot;
    private Vector3 _endPositionRightHand;
    private Vector3 _endPositionRightFoot;
    private Vector3 _endPositionLeftHand;

    private Vector3 rightFootPos;

    private float timeTakenDuringLerp = 0.2f;

    void StartLerpingRightFoot()
    {
        _isLerpingRF = true;
        _timeStartedLerpingRF = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionRightFoot = rightPoints[rightFootIndex - 1].position;
        _endPositionRightFoot = rightPoints[rightFootIndex].position;
    }

    void StartLerpingLeftFoot()
    {
        _isLerpingLF = true;
        _timeStartedLerpingLF = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionLeftFoot = leftPoints[leftFootIndex - 1].position;
        _endPositionLeftFoot = leftPoints[leftFootIndex].position;
    }

    void StartLerpingLeftHand()
    {
        _isLerpingLH = true;
        _timeStartedLerpingLH = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionLeftHand = leftHandPoints[leftHandIndex - 1].position;
        _endPositionLeftHand = leftHandPoints[leftHandIndex].position;
    }

    void StartLerpingRightHand()
    {
        _isLerpingRH = true;
        _timeStartedLerpingRH = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionRightHand = rightHandPoints[rightHandIndex - 1].position;
        _endPositionRightHand = rightHandPoints[rightHandIndex].position;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        startedAnimation = false;
        leftFootIndex = 0;
        rightFootIndex = 0;
        rightHandIndex = 0;
        leftHandIndex = 0;
        _isLerpingRF = false;
        _isLerpingLF = false;
        _isLerpingLH = false;
        _isLerpingRH = false;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("ladder"))
        {
            if (!startedAnimation)
            {
                startedAnimation = true;
                //StartLerpingLeftFoot();
                //StartLerpingRightFoot();
            }
            
        }
        else
        {
            startedAnimation = false;
            leftFootIndex = 0;
            rightFootIndex = 0;
            rightHandIndex = 0;
            leftHandIndex = 0;

            _isLerpingLF = false;
            _isLerpingLH = false;
            _isLerpingRF = false;
            _isLerpingRH = false;
        }

        //Debug.Log("Left: " + leftFootIndex);
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("ladder"))
        {
            rightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
            if (_isLerpingRF)
            {
                float timeSinceStarted = Time.time - _timeStartedLerpingRF;
                float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


                Vector3 newPos = Vector3.Lerp(_startPositionRightFoot, _endPositionRightFoot, percentageComplete);


                animator.SetIKPosition(AvatarIKGoal.RightFoot, newPos);

                if (newPos == _endPositionRightFoot)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, _endPositionRightFoot);
                    _isLerpingRF = false;
                }
            }
            else
            {
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightPoints[rightFootIndex].position);
            }

            if (_isLerpingLF)
            {
                float timeSinceStarted = Time.time - _timeStartedLerpingLF;
                float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


                Vector3 newPos = Vector3.Lerp(_startPositionLeftFoot, _endPositionLeftFoot, percentageComplete);


                animator.SetIKPosition(AvatarIKGoal.LeftFoot, newPos);

                if (newPos == _endPositionLeftFoot)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, _endPositionLeftFoot);
                    _isLerpingLF = false;
                }
            }
            else
            {
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftPoints[leftFootIndex].position);
            }

            if (_isLerpingLH)
            {
                float timeSinceStarted = Time.time - _timeStartedLerpingLH;
                float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


                Vector3 newPos = Vector3.Lerp(_startPositionLeftHand, _endPositionLeftHand, percentageComplete);


                animator.SetIKPosition(AvatarIKGoal.LeftHand, newPos);

                if (newPos == _endPositionLeftHand)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, _endPositionLeftHand);
                    _isLerpingLH = false;
                }
            }
            else
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPoints[leftHandIndex].position);
            }

            if (_isLerpingRH)
            {
                float timeSinceStarted = Time.time - _timeStartedLerpingRH;
                float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


                Vector3 newPos = Vector3.Lerp(_startPositionRightHand, _endPositionRightHand, percentageComplete);


                animator.SetIKPosition(AvatarIKGoal.RightHand, newPos);

                if (newPos == _endPositionRightHand)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightHand, _endPositionRightHand);
                    _isLerpingRH = false;
                }
            }
            else
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPoints[rightHandIndex].position);
            }

            // Set the look __target position__, if one has been assigned
            //if (lookObj != null)
            //{
            //    animator.SetLookAtWeight(1);
            //    animator.SetLookAtPosition(lookObj.position);
            //}

            // Set the right hand target position and rotation, if one has been assigned
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, positionWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, positionWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, positionWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, positionWeight);
            //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, positionWeight);
            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, positionWeight);
            //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);


            //animator.SetIKPosition(AvatarIKGoal.LeftHand, leftTwo.position);
            //animator.SetIKPosition(AvatarIKGoal.RightHand, rightTwo.position);
            //animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(rightHandObj.position - transform.position));
            

        }

        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animator.SetLookAtWeight(0);

            
        }
    }

    public void Left()
    {
        
        rightFootIndex++;
        rightHandIndex++;
        StartLerpingRightFoot();
        StartLerpingRightHand();
    }

    public void Right()
    {
        
        leftFootIndex++;
        leftHandIndex++;
        StartLerpingLeftFoot();
        StartLerpingLeftHand();
    }

    public void PunchOneEnded()
    {
        Salty_Rusty_Controller.punchOneEnded = true;
    }
}