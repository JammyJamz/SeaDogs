using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MediumGolemController : MonoBehaviour
{
    [HideInInspector]
    public bool slam;

    public Transform projectileStartPos;

    public GameObject projectile;

    public Animator anim;

    public Transform[] navMeshPoints;

    private bool reachedPoint;

    private int targetPoint;

    private FieldOfView fov;

    private bool wasJustInView;
    private bool wasJustPatrolling;

    private float timeTakenDuringSlerp = 1f;

    NavMeshAgent agent;

    bool _isSlerping;
    float _timeStartedLerping;
    Quaternion _startRotation;
    Quaternion _endRotation;

    void StartSlerping(Quaternion endRot)
    {
        _isSlerping = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startRotation = transform.rotation;
        _endRotation = endRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        slam = false;
        reachedPoint = false;
        float r = Random.value;
        if (r == 1f)
            r = 0.9f;
        targetPoint = (int)(r*10);
        agent.SetDestination(navMeshPoints[targetPoint].position);

        fov = GetComponent<FieldOfView>();
        wasJustInView = false;
        _isSlerping = false;
        wasJustPatrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Salty_Rusty_Controller.isSalty)
        {
            if(!fov.saltyInView)
            {
                wasJustPatrolling = true;
            }
            fov.FindVisibleTarget();

            if (fov.saltyInView && Vector3.Distance(Salty_Rusty_Controller.globalSalty.transform.position, transform.position) < 10f)
            {
                if(wasJustPatrolling)
                {
                    Quaternion rot = Quaternion.LookRotation(Salty_Rusty_Controller.globalSalty.transform.position - transform.position, Vector3.up);
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                    StartSlerping(rot);
                    wasJustPatrolling = false;
                }
                if(_isSlerping)
                {
                    float timeSinceStarted = Time.time - _timeStartedLerping;
                    float percentageComplete = timeSinceStarted / timeTakenDuringSlerp;

                    Quaternion rot = Quaternion.LookRotation(Salty_Rusty_Controller.globalSalty.transform.position - transform.position, Vector3.up);
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);

                    transform.rotation = Quaternion.Lerp(_startRotation, rot, percentageComplete);


                    if (transform.rotation == _endRotation)
                    {
                        _isSlerping = false;
                    }
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(Salty_Rusty_Controller.globalSalty.transform.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                agent.isStopped = true;
                anim.SetBool("isPatrolling", false);
            }
            else
            {
                agent.isStopped = false;
                anim.SetBool("isPatrolling", true);
            }

            if (!agent.isStopped)
            {
                if(!fov.saltyInView)
                {
                    if(wasJustInView)
                    {
                        reachedPoint = true;
                        wasJustInView = false;
                    }

                    if (Vector3.Distance(transform.position, navMeshPoints[targetPoint].position) < 0.5f)
                        reachedPoint = true;

                    if (reachedPoint)
                    {
                        float r = Random.value;
                        if (r == 1f)
                            r = 0.9f;
                        int newPoint = (int)(r * 10);

                        while (newPoint == targetPoint)
                        {
                            r = Random.value;
                            if (r == 1f)
                                r = 0.9f;
                            newPoint = (int)(r * 10);
                        }
                        targetPoint = newPoint;
                        reachedPoint = false;
                        Debug.Log(targetPoint);
                        agent.SetDestination(navMeshPoints[targetPoint].position);
                    }
                }
                else
                {
                    if (Vector3.Distance(Salty_Rusty_Controller.globalSalty.transform.position, transform.position) > 10f)
                    {
                        agent.SetDestination(Salty_Rusty_Controller.globalSalty.transform.position);
                    }
                    wasJustInView = true;
                }
            }
        }
        else
        {
            if (!fov.rustyInView)
            {
                wasJustPatrolling = true;
            }
            fov.FindVisibleTarget();

            if (fov.rustyInView && Vector3.Distance(Salty_Rusty_Controller.globalRusty.transform.position, transform.position) < 10f)
            {
                if (wasJustPatrolling)
                {
                    Quaternion rot = Quaternion.LookRotation(Salty_Rusty_Controller.globalRusty.transform.position - transform.position, Vector3.up);
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                    StartSlerping(rot);
                    wasJustPatrolling = false;
                }
                if (_isSlerping)
                {
                    float timeSinceStarted = Time.time - _timeStartedLerping;
                    float percentageComplete = timeSinceStarted / timeTakenDuringSlerp;

                    Quaternion rot = Quaternion.LookRotation(Salty_Rusty_Controller.globalRusty.transform.position - transform.position, Vector3.up);
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);

                    transform.rotation = Quaternion.Lerp(_startRotation, rot, percentageComplete);


                    if (transform.rotation == _endRotation)
                    {
                        _isSlerping = false;
                    }
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(Salty_Rusty_Controller.globalRusty.transform.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                agent.isStopped = true;
                anim.SetBool("isPatrolling", false);
            }
            else
            {
                agent.isStopped = false;
                anim.SetBool("isPatrolling", true);
            }

            if (!agent.isStopped)
            {
                if (!fov.rustyInView)
                {
                    if (wasJustInView)
                    {
                        reachedPoint = true;
                        wasJustInView = false;
                    }

                    if (Vector3.Distance(transform.position, navMeshPoints[targetPoint].position) < 0.5f)
                        reachedPoint = true;

                    if (reachedPoint)
                    {
                        float r = Random.value;
                        if (r == 1f)
                            r = 0.9f;
                        int newPoint = (int)(r * 10);

                        while (newPoint == targetPoint)
                        {
                            r = Random.value;
                            if (r == 1f)
                                r = 0.9f;
                            newPoint = (int)(r * 10);
                        }
                        targetPoint = newPoint;
                        reachedPoint = false;
                        Debug.Log(targetPoint);
                        agent.SetDestination(navMeshPoints[targetPoint].position);
                    }
                }
                else
                {
                    if (Vector3.Distance(Salty_Rusty_Controller.globalRusty.transform.position, transform.position) > 10f)
                    {
                        agent.SetDestination(Salty_Rusty_Controller.globalRusty.transform.position);
                    }
                    wasJustInView = true;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if(slam)
        {
            Instantiate(projectile, projectileStartPos.position, transform.rotation);
            slam = false;
        }
    }

}
