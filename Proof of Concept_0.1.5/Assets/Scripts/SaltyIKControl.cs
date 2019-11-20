using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltyIKControl : MonoBehaviour
{

    public static bool saltyLeftHandInTrigger;
    public static bool saltyRightHandInTrigger;

    public float positionWeight;

    public static Vector3 ledgePosition;

    public Transform saltyModel;

    public Transform saltyBlunderLeftHandPos;

    private Animator animator;

    private bool enteredLedgeGrab;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        saltyLeftHandInTrigger = false;
        saltyRightHandInTrigger = false;
        enteredLedgeGrab = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (saltyLeftHandInTrigger || saltyRightHandInTrigger)
        {
            enteredLedgeGrab = true;
        }
    }

    void OnAnimatorIK()
    {
        if ((animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("climbing") || animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("hanging")) && enteredLedgeGrab)
        {


            Vector3 tempPos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            Vector3 tempPos2 = animator.GetIKPosition(AvatarIKGoal.RightHand);

            tempPos = new Vector3(tempPos.x, ledgePosition.y, tempPos.z);
            //tempPos2 = new Vector3(tempPos2.x, ledgePosition.position.y, tempPos2.z);

            RaycastHit hit;
            Physics.Raycast(transform.position, saltyModel.forward, out hit, 3f, LayerMask.GetMask("Climable Walls"));

            Vector3 hitPos = hit.point;

            tempPos = new Vector3(hitPos.x, tempPos.y, hitPos.z) + transform.right*-1/2;
            tempPos2 = new Vector3(hitPos.x, tempPos.y, hitPos.z) + transform.right*1/2;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, positionWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, positionWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, positionWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, positionWeight);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.Lerp(animator.GetIKPosition(AvatarIKGoal.LeftHand), tempPos, 1f));
            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(transform.forward + transform.up));
            animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(transform.forward + transform.up));
            animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.Lerp(animator.GetIKPosition(AvatarIKGoal.RightHand), tempPos2, 1f));
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("aiming"))
        {
            enteredLedgeGrab = false;
            animator.SetLookAtPosition(Camera.main.transform.position + Camera.main.transform.forward*10f);
            Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 10f, Color.red);
            animator.SetLookAtWeight(1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, positionWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.Lerp(animator.GetIKPosition(AvatarIKGoal.LeftHand), saltyBlunderLeftHandPos.position, 1f));
        }
        else
        {
            enteredLedgeGrab = false;
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        }
    }
}
