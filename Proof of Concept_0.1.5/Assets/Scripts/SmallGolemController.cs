using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallGolemController : MonoBehaviour
{
    [HideInInspector]
    public bool slam;

    public float runningSpeed = 5.5f;
    public float walkingSpeed = 3.5f;

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
        targetPoint = (int)(r * 10);
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
            Vector3 targetDir = transform.position - Salty_Rusty_Controller.globalSalty.transform.position;
            Vector3 targetPos = Salty_Rusty_Controller.globalSalty.transform.position + targetDir.normalized;
            if (!fov.saltyInView)
            {
                wasJustPatrolling = true;
            }
            fov.FindVisibleTarget();
            if (fov.saltyInView)
            {
                if (wasJustPatrolling)
                {
                    Quaternion rot = Quaternion.LookRotation(Salty_Rusty_Controller.globalSalty.transform.position - transform.position, Vector3.up);
                    rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                    StartSlerping(rot);
                    wasJustPatrolling = false;
                }
                if (_isSlerping)
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
                    transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                
                anim.SetBool("isPatrolling", false);
            }
            else
            {
                anim.SetBool("isPatrolling", true);
            }

            if (anim.GetBool("isPatrolling"))
            {
                anim.SetBool("startAttack", false);
                anim.SetBool("isChasing", false);
                agent.speed = walkingSpeed;
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
                if (anim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("attack"))
                    agent.speed = runningSpeed;
                else
                    agent.speed = 0f;

                agent.SetDestination(targetPos);

                if (Vector3.Distance(transform.position, Salty_Rusty_Controller.globalSalty.transform.position) < 2f)
                {
                    anim.SetBool("startAttack", true);
                    anim.SetBool("isChasing", false);
                }
                else
                {
                    anim.SetBool("startAttack", false);
                    anim.SetBool("isChasing", true);
                }

                wasJustInView = true;
            }
        }
        else
        {
            Vector3 targetDir = transform.position - Salty_Rusty_Controller.globalRusty.transform.position;
            Vector3 targetPos = Salty_Rusty_Controller.globalRusty.transform.position + targetDir.normalized;
            if (!fov.rustyInView)
            {
                wasJustPatrolling = true;
            }
            fov.FindVisibleTarget();
            if (fov.rustyInView)
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
                    transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }

                anim.SetBool("isPatrolling", false);
            }
            else
            {
                anim.SetBool("isPatrolling", true);
            }

            if (anim.GetBool("isPatrolling"))
            {
                anim.SetBool("startAttack", false);
                anim.SetBool("isChasing", false);
                agent.speed = walkingSpeed;
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
                if (anim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("attack"))
                    agent.speed = runningSpeed;
                else
                    agent.speed = 0f;

                agent.SetDestination(targetPos);

                if (Vector3.Distance(transform.position, Salty_Rusty_Controller.globalRusty.transform.position) < 2f)
                {
                    anim.SetBool("startAttack", true);
                    anim.SetBool("isChasing", false);
                }
                else
                {
                    anim.SetBool("startAttack", false);
                    anim.SetBool("isChasing", true);
                }
                wasJustInView = true;
            }
        }

    }

    public void LateUpdate()
    {
        anim.SetBool("tookDamage", false);
    }

    private void FixedUpdate()
    {

    }

    public void Hit()
    {
        anim.Play("Small_Golem_FLinch", 0, 0);
    }
}
