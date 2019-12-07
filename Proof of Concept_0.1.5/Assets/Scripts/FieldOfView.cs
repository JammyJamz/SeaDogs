using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;

    [Range(0, 360)]
    public float viewAngle;

    public LayerMask saltyMask;
    public LayerMask rustyMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public bool saltyInView;

    [HideInInspector]
    public bool rustyInView;

    private void Start()
    {
        saltyInView = false;
        rustyInView = false;
    }

    public void FindVisibleTarget()
    {
        if(Salty_Rusty_Controller.isSalty)
        {
            saltyInView = false;
            Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position + Vector3.up*0.5f, viewRadius, saltyMask);
            for (int i = 0; i < targetsinViewRadius.Length; i++)
            {
                if (targetsinViewRadius[i].gameObject.tag == "saltyCollider")
                {
                    
                    Vector3 targetsPos = Salty_Rusty_Controller.globalSalty.transform.position + Vector3.up*0.5f;
                    Vector3 dirToTarget = (targetsPos - (transform.position + Vector3.up * 0.5f)).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dist = Vector3.Distance((transform.position + Vector3.up * 0.5f), targetsPos);
                        if (!Physics.Raycast((transform.position + Vector3.up * 0.5f), dirToTarget, dist, obstacleMask, QueryTriggerInteraction.Ignore))
                        {
                            saltyInView = true;
                        }
                    }
                }
            }

        }
        else
        {
            rustyInView = false;
            Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, viewRadius, rustyMask);

            for (int i = 0; i < targetsinViewRadius.Length; i++)
            {
                if (targetsinViewRadius[i].gameObject.tag == "rustyCollider")
                {
                    Vector3 targetsPos = Salty_Rusty_Controller.globalRusty.transform.position + Vector3.up * 0.5f;
                    Vector3 dirToTarget = (targetsPos - (transform.position + Vector3.up * 0.5f)).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dist = Vector3.Distance(transform.position + Vector3.up * 0.5f, targetsPos);

                        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToTarget, dist, obstacleMask, QueryTriggerInteraction.Ignore))
                        {
                            rustyInView = true;
                        }
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
