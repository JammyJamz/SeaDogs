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
            Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position, viewRadius, saltyMask);

            for (int i = 0; i < targetsinViewRadius.Length; i++)
            {
                if (targetsinViewRadius[i].gameObject.tag == "saltyCollider")
                {
                    Transform target = Salty_Rusty_Controller.globalSalty.transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dist = Vector3.Distance(transform.position, target.position);

                        if(!Physics.Raycast(transform.position, dirToTarget, dist, obstacleMask, QueryTriggerInteraction.Ignore))
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
            Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position, viewRadius, rustyMask);

            for (int i = 0; i < targetsinViewRadius.Length; i++)
            {
                if (targetsinViewRadius[i].gameObject.tag == "rustyCollider")
                {
                    Transform target = Salty_Rusty_Controller.globalRusty.transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dist = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, dirToTarget, dist, obstacleMask, QueryTriggerInteraction.Ignore))
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
