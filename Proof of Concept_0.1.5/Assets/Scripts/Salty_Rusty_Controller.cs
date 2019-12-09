using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Salty_Rusty_Controller : MonoBehaviour
{
    public Transform rustyAimPosition;
    public AudioSource rustyPunchSound;
    public AudioSource rustyPunchSound2;
    public AudioSource saltyJumpSound;
    public AudioSource rustyJumpSound;

    public Transform spawnPos;
    public Transform rustySpawnPos;
    public Transform menuSpawnPos;

    public static bool usingXboxController;
    public static bool inThrowTrigger;
    public float runningSpeed = 15f;
    public float mouseSens = 15f;
    public float climbingSpeed = 15f;
    public float wallOffset = 0.5f;
    public float turnThresh = 0.5f;
    public float fallingSpeed = 1.5f;
    public float pushSpeed = 0.3f;
    public float aimWalkSpeed = 3f;

    public float meleeSnapSpeed = 4f;

    public Transform cameraPivot;
    public GameObject salty;
    public GameObject rusty;
    public Transform saltyModelTrans;
    public Transform rustyModelTans;
    public Transform saltyPivotPos;
    public Transform rustyPivotPos;
    public NavMeshAgent saltyAgent;
    public NavMeshAgent rustyAgent;
    public Transform sCollider;
    public Transform rCollider;
    public Transform pushCollider;
    public Transform climbCollider;
    public Transform camPivotPlaceHolder;
    public Transform saltyModelPlaceHolder;
    public Transform pivotPos;

    public GameObject saltyRing;
    public GameObject rustyRing;
    public GameObject blunderbuss;


    public Transform rustyRightHand;
    public Transform rustyLeftHand;

    public Transform camAimPos;

    public static Vector3 meleeTargetPosition;
    public static bool targetInRange;
    public static bool isSalty;
    public static bool saltyLeftHandInTrigger;
    public static bool saltyRightHandInTrigger;

    public static bool punchOneEnded;

    public static Transform globalSalty;
    public static Transform globalRusty;

    public static bool saltyOnPlatform;
    public static bool rustyOnPlatform;

    public Animator rustyAnim;
    public Animator saltyAnim;

    public GameObject saltyThrowVFX;


    private Rigidbody saltyRig, rustyRig;
    private Transform saltyPos, rustyPos, currentCharacter;
    private float movX, movZ, mouseX, mouseY;
    private Vector3 fixRotation, posToMove, velocity, nonClimbingRot, currentWallDir, up;
    private RaycastHit startHit, lastHit;
    private Quaternion exitRotationTarget;
    private bool climbStarted, isFalling, isLerping, switchKeyDown, isSwitching, isClimbing, climbKeyDown, throwKeyDown, inRustyHands, jumpKeyDown, saltyIsFalling, rustyIsFalling;
    private bool throwActivated;
    private bool isBeingThrown;
    private bool startSwitchVFX;

    private bool punchKeyDown;
    private bool punchActivated;
    private bool secondPunchActivated;

    private bool jumpActivated;

    private bool rustyIsClimbing;

    private float aiTimer;

    private float vfxTimer;

    private float rotInc;

    private float timeTakenDuringLerp = 0.25f;
    private float saltyTimeTakenDuringLerp = 0.25f;

    private Quaternion newRot;
    private Quaternion newRot2;

    public static bool inPunchAnimation;
    public static bool inPunchAnimationTwo;

    public static bool isAiming;

    //Whether we are currently interpolating or not
    private bool _isLerping;
    private bool _isLerpingAim;
    private bool _saltyIsLerping;

    //The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _startPositionAim;
    private Vector3 _endPosition;
    private Vector3 _endPositionAim;
    private Vector3 _saltyStartPos;
    private Vector3 _saltyEndPos;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;
    private float _timeStartedLerpingAim;
    private float _saltyTimeStartedLerping;

    private bool enteredLedgeGrab;

    private float camOffset = -4.019f;
    private float camOffsetAim = -1.5f;

    private Vector3 camStartLocalPos;

    private Camera cam;

    private CameraScript camScript;

    private float offset;

    public static float throwForceMagnitude;
    public static Transform endThrowPoint;

    float lastRightTrigger;
    float lastLeftTrigger;

    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = camPivotPlaceHolder.localPosition;
        _endPosition = rustyPivotPos.localPosition;
    }

    void StartLerpingCamAim(Vector3 startPos, Vector3 endPos)
    {
        _isLerpingAim = true;
        _timeStartedLerpingAim = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionAim = startPos;
        _endPositionAim = endPos;
    }

    void StartLerpingSalty(Vector3 startPos, Vector3 endPos)
    {
        _saltyIsLerping = true;
        _saltyTimeStartedLerping = Time.time;

        _saltyStartPos = startPos;
        _saltyEndPos = endPos;

    }

    private void Start()
    {
        lastRightTrigger = 0;
        lastLeftTrigger = 0;
        usingXboxController = false;
        if (SceneManager.GetActiveScene().name == "White Box 1" && PlayerPrefs.GetInt("loadedFromMenu", 0) == 1)
        {
            transform.position = menuSpawnPos.position;
            PlayerPrefs.SetInt("loadedFromMenu", 0);
        }
        else if (SceneManager.GetActiveScene().name == "White Box 1" && PlayerPrefs.GetInt("loadedFromMenu", 0) != 1)
        {
            transform.position = spawnPos.position;
            rustyAgent.Warp(rustySpawnPos.position);
        }

        cam = Camera.main;
        camScript = GetComponent<CameraScript>();

        Application.targetFrameRate = 300;
        // initialization of variables
        currentCharacter = (new GameObject()).transform;
        rustyRig = rusty.GetComponent<Rigidbody>();
        saltyRig = salty.GetComponent<Rigidbody>();

        //rustyAnim = rusty.GetComponent<Animator>();
        //saltyAnim = salty.GetComponent<Animator>();

        rustyAgent.enabled = true;
        saltyAgent.enabled = false;
        isSalty = true;
        ButtonScript.isSalty = true;
        movX = 0f;
        movZ = 0f;
        mouseX = 0f;
        mouseY = 0f;
        velocity = Vector3.zero;
        isClimbing = false;
        climbKeyDown = false;
        up = new Vector3(0, 1, 0);
        climbStarted = false;
        isFalling = false;
        saltyPos = salty.transform;
        rustyPos = rusty.transform;
        isLerping = false;

        throwKeyDown = false;
        inRustyHands = false;
        throwActivated = false;
        isBeingThrown = false;
        isSwitching = true;
        isSalty = true;
        inThrowTrigger = false;
        ButtonScript.isSalty = true;

        // update parent of cam pivot placeholder
        camPivotPlaceHolder.SetParent(salty.transform);

        // update Nav Agents
        rustyAgent.enabled = true;
        saltyAgent.enabled = false;
        rCollider.gameObject.SetActive(false);
        sCollider.gameObject.SetActive(true);

        nonClimbingRot = new Vector3(cameraPivot.localRotation.eulerAngles.x, cameraPivot.localRotation.eulerAngles.y, cameraPivot.localRotation.eulerAngles.z);
        exitRotationTarget = Quaternion.Euler(nonClimbingRot.x, nonClimbingRot.y, nonClimbingRot.z);

        rustyRing.SetActive(false);
        saltyRing.SetActive(false);

        vfxTimer = 0f;
        aiTimer = 0f;

        jumpKeyDown = false;
        jumpActivated = false;

        rustyIsClimbing = false;
        pushCollider.gameObject.SetActive(false);
        _isLerping = false;
        _isLerpingAim = false;
        _saltyIsLerping = false;

        newRot = saltyModelTrans.rotation;
        newRot2 = saltyModelTrans.rotation;

        Vector3 tempVert = salty.transform.forward * 1;
        Vector3 tempHor = (salty.transform.right) * 1;

        Quaternion rot = Quaternion.LookRotation((tempVert + tempHor).normalized, Vector3.up);

        rotInc = (salty.transform.rotation.eulerAngles.y - rot.eulerAngles.y) / 2;

        punchKeyDown = false;
        punchActivated = false;
        secondPunchActivated = false;

        meleeTargetPosition = Vector3.zero;
        targetInRange = false;

        inPunchAnimation = false;
        inPunchAnimationTwo = false;

        saltyLeftHandInTrigger = false;
        saltyRightHandInTrigger = false;

        enteredLedgeGrab = false;

        punchOneEnded = false;

        isAiming = false;

        camStartLocalPos = cam.transform.localPosition;

        blunderbuss.SetActive(false);

        globalRusty = rusty.transform;
        globalSalty = salty.transform;

        saltyOnPlatform = false;
        rustyOnPlatform = false;

        saltyIsFalling = false;
        rustyIsFalling = false;

        HealthBarController.currentHealth = 3;
    }


    private void Update()
    {
        if(PauseMenu.GameIsPaused)
        {
            isAiming = false;
            return;
        }
        if (Input.GetButtonDown("Xbox Start") || Input.GetButtonDown("Xbox Select") || Input.GetButtonDown("Xbox RS") ||
           Input.GetButtonDown("Xbox LS") || Input.GetButtonDown("Xbox B") || Input.GetButtonDown("Xbox LB") ||
           Input.GetButtonDown("Xbox RB") || Input.GetButtonDown("Xbox Interact") || Input.GetButtonDown("Xbox Jump") ||
           Input.GetButtonDown("Xbox Character Switch") || Input.GetButtonDown("Xbox Start") || Input.GetAxisRaw("Xbox Right Trigger") > 0 ||
           Input.GetAxisRaw("Xbox Left Trigger") > 0 || Input.GetAxisRaw("Xbox Dpad Up Down") != 0 || Input.GetAxisRaw("Xbox Dpad Left Right") != 0 ||
           Input.GetAxisRaw("Controller X") != 0 || Input.GetAxisRaw("Controller Y") != 0 || Input.GetAxisRaw("Xbox Vertical") != 0 ||
           Input.GetAxisRaw("Xbox Horizontal") != 0)
        {
            usingXboxController = true;
        }
        else
        {
            if (Input.anyKeyDown)
                usingXboxController = false;
            if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
                usingXboxController = false;
        }
        // reset vars
        climbKeyDown = false;

        //rustyAnim.SetBool("secondPunchActivated", false);

        // get movement input
        if(!usingXboxController)
        {
            movZ = Input.GetAxisRaw("Vertical");
            movX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            movX = Input.GetAxisRaw("Xbox Horizontal");
            movZ = Input.GetAxisRaw("Xbox Vertical");
        }

        // get button inputs
        switchKeyDown = Input.GetButtonDown("PC Character Switch");
        if(!switchKeyDown)
        {
            switchKeyDown = Input.GetButtonDown("Xbox Character Switch");
        }

        climbKeyDown = Input.GetButtonDown("PC Interact");
        if(!climbKeyDown)
        {
            climbKeyDown = Input.GetButtonDown("Xbox Interact");
        }

        throwKeyDown = Input.GetButtonDown("PC Interact");
        if (!throwKeyDown)
        {
            throwKeyDown = Input.GetButtonDown("Xbox Interact");
        }

        jumpKeyDown = Input.GetButtonDown("PC Jump");
        if(!jumpKeyDown)
        {
            jumpKeyDown = Input.GetButtonDown("Xbox Jump");
        }


        float rightTriggerInput = Input.GetAxisRaw("Xbox Right Trigger");
        bool triggerDown = false;

        if (lastRightTrigger == 0 && rightTriggerInput > 0)
            triggerDown = true;

        punchKeyDown = Input.GetMouseButtonDown(0);
        if(!punchKeyDown)
        {
            punchKeyDown = triggerDown;
        }

        // get camera input
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        mouseX += Input.GetAxis("Controller X") * Time.deltaTime * mouseSens;

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;

        if (jumpKeyDown && !isAiming)
        {
            if(isSalty && saltyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("falling"))
                jumpActivated = true;

            if (!isSalty)
                jumpActivated = true;
        }

        bool aimKeyDown = Input.GetMouseButtonDown(1);
        bool leftTriggerDown = false;
        float leftTriggerInput = Input.GetAxisRaw("Xbox Left Trigger");

        if (lastLeftTrigger == 0 && leftTriggerInput > 0)
            leftTriggerDown = true;
        if (!aimKeyDown)
            aimKeyDown = leftTriggerDown;

        bool aimKeyUp = Input.GetMouseButtonUp(1);
        bool leftTriggerUp = false;

        if (lastLeftTrigger >= 0 && leftTriggerInput == 0)
            leftTriggerUp = true;
        if (!aimKeyUp && usingXboxController)
            aimKeyUp = leftTriggerUp;


        if (aimKeyDown && isSalty && !saltyIsFalling)
        {
            isAiming = true;
            StartLerpingCamAim(cam.transform.localPosition, camAimPos.localPosition);
            //cam.transform.localPosition = camAimPos.localPosition;
            //camScript.maxCamOffset = -1.5f;
            //camScript.camOffset = -1.5f;

            offset = -1.5f;
            saltyAnim.SetBool("isAiming", true);

            Quaternion tempRot = saltyModelTrans.rotation;
            salty.transform.rotation = tempRot;
            saltyModelTrans.rotation = tempRot;
            blunderbuss.SetActive(true);
        }
        else if(aimKeyUp && isSalty && !saltyIsFalling && isAiming)
        {
            isAiming = false;          
            StartLerpingCamAim(camAimPos.localPosition, camStartLocalPos);
            //cam.transform.localPosition = camStartLocalPos;
            //camScript.maxCamOffset = camOffset;
            //camScript.camOffset = camOffset;

            
            saltyAnim.SetBool("isAiming", false);
            blunderbuss.gameObject.GetComponent<BlunderbussScript>().BlunderVFX.gameObject.SetActive(false);
            blunderbuss.SetActive(false);
        }

        // move camera from character to other character
        if (_isLerpingAim)
        {
            float timeSinceStarted = Time.time - _timeStartedLerpingAim;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


            cam.transform.localPosition = Vector3.Lerp(_startPositionAim, _endPositionAim, percentageComplete);

            if(isAiming)
            {
                camScript.camOffset = Mathf.Lerp(camOffset, offset, percentageComplete);
                camScript.maxCamOffset = Mathf.Lerp(camOffset, offset, percentageComplete);
            }
            else
            {
                camScript.camOffset = Mathf.Lerp(camOffsetAim, camOffset, percentageComplete);
                camScript.maxCamOffset = Mathf.Lerp(camOffsetAim, camOffset, percentageComplete);
            }
            


            if (cam.transform.localPosition == _endPositionAim)
            {
                _isLerpingAim = false;
            }
        }

        // switch key pressed
        if (switchKeyDown && !isClimbing && !inRustyHands && !isBeingThrown && !inPunchAnimation && !inPunchAnimationTwo && !isAiming)
        {
            // salty is not climbing so allowed to switch

            if (isSalty)
            {
                // switch to rusty

                isSalty = false;
                ButtonScript.isSalty = false;

                // update parent of cam pivot placeholder to rusty's model
                camPivotPlaceHolder.SetParent(rusty.transform);


                // update Nav Agents
                if (!saltyOnPlatform && !saltyIsFalling)
                {
                    saltyAgent.enabled = true;
                    sCollider.gameObject.SetActive(false);
                }
                
                rustyAgent.enabled = false;
                
                // update colliders
                
                rCollider.gameObject.SetActive(true);
            }
            else
            {
                // switch to salty
                isSalty = true;
                ButtonScript.isSalty = true;

                // update parent of cam pivot placeholder to salty's model
                camPivotPlaceHolder.SetParent(salty.transform);

                // update Nav Agents
                
                if (!rustyOnPlatform && !rustyIsFalling)
                {
                    rustyAgent.enabled = true;
                    rCollider.gameObject.SetActive(false);
                }

                saltyAgent.enabled = false;

                // update colliders
                sCollider.gameObject.SetActive(true);
            }

            // update isSwitching bool
            isSwitching = true;
            StartLerping();

            // reset switchKeyDown
            switchKeyDown = false;
            startSwitchVFX = true;
            vfxTimer = 0f;
            saltyRing.SetActive(false);
            rustyRing.SetActive(false);
        }
        else if (climbKeyDown && isSalty && !isAiming) // climb key pressed
        {
            // isSalty so can start climb

            if (!isClimbing) // not currently climbing
            {
                // raycast in front of salty to see if it hits climable wall
                if (Physics.Raycast(saltyModelTrans.position + new Vector3(0, 0.5f, 0), saltyModelTrans.forward, out RaycastHit hit, 0.8f, LayerMask.GetMask("Climable Walls")))
                {
                    // save hit and update climb bools
                    startHit = hit;
                    isClimbing = true;
                    climbStarted = true;
                    saltyAnim.SetBool("isClimbing", true);

                    // check if in the air
                    if (saltyIsFalling)
                    {
                        // update falling bools
                        saltyAnim.SetTrigger("grabbedWallInAir");

                        // grabbed a wall so no longer falling
                        saltyIsFalling = false;
                        saltyAnim.SetBool("isFalling", false);
                    }

                    // save current direction to the wall that was hit
                    currentWallDir = hit.normal * -1;

                    // turn gravity off for salty
                    saltyRig.useGravity = false;

                    // turn salty collider off, reset velocity, turn climb collider on (so does not go through the floor)
                    sCollider.GetComponent<CapsuleCollider>().isTrigger = true;
                    saltyRig.velocity = Vector3.zero;
                    climbCollider.gameObject.SetActive(true);
                }
            }
            else // is climbing when pressed, so need to drop off wall
            {
                UpdateSaltyAfterClimb();
            }
        }
        else if (throwKeyDown && !isSalty && !inRustyHands && inThrowTrigger)
        {
            // throw key was pressed by rusty in the throw trigger

            // update bools
            inRustyHands = true;
            sCollider.gameObject.SetActive(false);
            saltyAnim.SetBool("isReadyForThrow", true);
            rustyRig.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            rustyAnim.SetFloat("PosZ", 0f);

            Debug.DrawRay(Vector3.Lerp(rustyRightHand.position, rustyLeftHand.position, 0.5f), Vector3.up, Color.red, 20f);
            StartLerpingSalty(salty.transform.position, Vector3.Lerp(rustyRightHand.position, rustyLeftHand.position, 0.5f) + (rustyModelTans.forward * 0.25f) - Vector3.up*0.5f);

            // don't want salty moving on navmesh when in rusty's hands
            saltyAgent.enabled = false;
        }
        else if (throwKeyDown && !isSalty && inRustyHands)
        {
            // salty in rusty's hands and throw key was pressed so initiate throw

            // update bools
            throwActivated = true;
            inRustyHands = false;
            // reset anim bool
            saltyAnim.SetBool("isReadyForThrow", false);
        }

        if(punchKeyDown && !isSalty && !rustyIsClimbing && !inRustyHands && !rustyIsFalling)
        {
            

            if (rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("punch_1") && rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("punch_2"))
            {
                
                secondPunchActivated = true;
                //rustyAnim.SetBool("secondPunchActivated", true);
            }
            else if(rustyAnim.GetAnimatorTransitionInfo(0).userNameHash != Animator.StringToHash("punch_1") && rustyAnim.GetAnimatorTransitionInfo(0).userNameHash != Animator.StringToHash("punch_2") && rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("punch_1") && rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("punch_2"))
            {
                rustyPunchSound2.Stop();
                rustyPunchSound.Stop();
                rustyPunchSound.Play();
                punchActivated = true;
                rustyAnim.SetBool("punchActivated", true);
                rustyRig.velocity = Vector3.zero;
            }
        }

        if(secondPunchActivated && punchOneEnded)
        {
            rustyPunchSound.Stop();
            rustyPunchSound2.Stop();
            rustyPunchSound2.Play();
            rustyAnim.SetBool("secondPunchActivated", true);
            //secondPunchActivated = false;
        }

        if(punchOneEnded)
        {
            punchOneEnded = false;
        }

        /*if(!punchKeyDown && punchActivated)
        {
            punchActivated = false;
            rustyAnim.SetBool("punchActivated", false);
        }*/


        // reset keys
        throwKeyDown = false;
        climbKeyDown = false;
        jumpKeyDown = false;
        punchKeyDown = false;

        Vector3 movVertical;
        Vector3 movHorizontal;

        if (startSwitchVFX)
        {

            if (isSalty)
            {
                saltyRing.SetActive(true);
            }
            else
            {
                rustyRing.SetActive(true);
            }

            vfxTimer += Time.deltaTime;

            if (vfxTimer >= 1.5f)
            {
                vfxTimer = 0f;
                startSwitchVFX = false;
                rustyRing.SetActive(false);
                saltyRing.SetActive(false);
            }
        }

        if (isSwitching)
        {
            // move camera from character to other character
            if(_isLerping)
            {
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

               
                camPivotPlaceHolder.localPosition = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

                    
                if (camPivotPlaceHolder.localPosition == _endPosition)
                {
                    isSwitching = false;
                    _isLerping = false;
                }
            }
            
        }

        if (!isClimbing && !inRustyHands && !throwActivated)
        {
            // not climbing or throwing

            // reset anim trigger for grabbing wall in air
            saltyAnim.ResetTrigger("grabbedWallInAir");

            // **** CURRENTLY NOT ROTATING CHARACTER TRANS WHEN CLIMBING ****
            // lerps salty rotation back to vertical since no longer climbing
            // salty.transform.rotation = Quaternion.Lerp(salty.transform.rotation, Quaternion.Euler(0, salty.transform.rotation.eulerAngles.y, 0), 0.02f);

            // update current character position and rotation
            currentCharacter.position = camPivotPlaceHolder.position;
            currentCharacter.rotation = cameraPivot.rotation;


            // zero out X and Z rotation for movement vectors
            currentCharacter.rotation = Quaternion.Euler(0, currentCharacter.eulerAngles.y, 0);

            // set movement vectors
            movVertical = currentCharacter.forward * movZ;
            movHorizontal = currentCharacter.right * movX;

            // set velocity vector direction
            velocity = (movVertical + movHorizontal).normalized;


            // check to see if moving
            if ((velocity != Vector3.zero && !inPunchAnimation && !inPunchAnimationTwo) || (targetInRange && (inPunchAnimation || inPunchAnimationTwo)))
            {
                // get new rotation based of movement vectors

                if(isSalty)
                {
                    newRot = Quaternion.LookRotation((movVertical + movHorizontal).normalized, Vector3.up);
                    newRot2 = Quaternion.Euler(pushCollider.eulerAngles.x, newRot.eulerAngles.y, pushCollider.eulerAngles.z);
                }
                else
                {
                    if ((inPunchAnimation || inPunchAnimationTwo) && targetInRange)
                    {
                        
                        Debug.DrawRay(rusty.transform.position, (meleeTargetPosition - rusty.transform.position).normalized, Color.cyan);


                        newRot = Quaternion.LookRotation((meleeTargetPosition - rusty.transform.position).normalized, Vector3.up);
                        newRot = Quaternion.Euler(0, newRot.eulerAngles.y, 0);
                    }
                    else 
                    {
                        newRot = Quaternion.LookRotation((movVertical + movHorizontal).normalized, Vector3.up);
                    }


                    newRot2 = Quaternion.Euler(pushCollider.eulerAngles.x, newRot.eulerAngles.y, pushCollider.eulerAngles.z);
                }
                

                // lerp rotation of current character to new rotation
                if(isSalty && !isAiming)
                {
                    saltyModelTrans.rotation = Quaternion.Slerp(saltyModelTrans.rotation, newRot, 7 * Time.deltaTime);
                }
                if(!isSalty)
                {
                    rustyModelTans.rotation = Quaternion.Slerp(rustyModelTans.rotation, newRot, 7 * Time.deltaTime);
                    pushCollider.rotation = Quaternion.Slerp(pushCollider.rotation, newRot2, 7 * Time.deltaTime);
                }
            }

            if (isSalty && !isAiming)
            {
                // lerps salty model and collider rotation back to vertical rotation since not climbing
                saltyModelTrans.rotation = Quaternion.Slerp(saltyModelTrans.rotation, Quaternion.Euler(0, saltyModelTrans.eulerAngles.y, 0), 7 * Time.deltaTime);
                sCollider.rotation = saltyModelTrans.rotation;
            }

            aiTimer += Time.deltaTime;

            if (aiTimer >= 0f)
            {
                if (!inRustyHands && !throwActivated)
                {
                    // update destinations of NavMesh Agents
                    if (isSalty)
                    {
                        if(rustyAgent.enabled && !isAiming)
                            rustyAgent.SetDestination(saltyRig.position);
                        else if (rustyAgent.enabled && isAiming)
                            rustyAgent.SetDestination(rustyAimPosition.position);

                    }
                    else
                    {
                        // update destination of salty AI
                        if(saltyAgent.enabled)
                            saltyAgent.SetDestination(rustyRig.position);
                    }
                }
                aiTimer = 0f;
            }

            if (isSalty)
            {
                // update anim move vars
                if(!isAiming && !isClimbing)
                {
                    saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
                    saltyAnim.SetFloat("PosX", 0f);
                }
                else
                {
                    saltyAnim.SetFloat("PosX", movX);
                    saltyAnim.SetFloat("PosZ", movZ);
                }
                
                rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(rustyAgent.velocity.magnitude), 0f, 1f));

                // AI model rotation
                Quaternion newRot = Quaternion.LookRotation(rusty.transform.forward, Vector3.up);
                rustyModelTans.rotation = Quaternion.Lerp(rustyModelTans.rotation, newRot, 7 * Time.deltaTime);
            }
            else
            {
                // AI model rotation
                Quaternion newRot = Quaternion.LookRotation(salty.transform.forward, Vector3.up);
                saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, newRot, 7 * Time.deltaTime);
            }
        }
        else if (inRustyHands)
        {
            if(_saltyIsLerping)
            {
                float timeSinceStarted = Time.time - _saltyTimeStartedLerping;
                float percentageComplete = timeSinceStarted / saltyTimeTakenDuringLerp;


                salty.transform.position = Vector3.Lerp(_saltyStartPos, _saltyEndPos, percentageComplete);


                if (salty.transform.position == _saltyEndPos)
                {
                    isSwitching = false;
                    _saltyIsLerping = false;
                }

            }

            // lerp salty's position to center of rusty's hands
            //salty.transform.position = Vector3.Lerp(salty.transform.position, Vector3.Lerp(rustyRightHand.position, rustyLeftHand.position, 0.5f) + (rustyModelTans.forward * 0.25f), 0.05f);

            // lerp salty's model rotation to rusty's rotation
            saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, rustyModelTans.rotation, 5f * Time.deltaTime);
        }
        else if (isSalty && isClimbing)
        {
            if(velocity == Vector3.zero)
            {
                // update climb movement anim bools
                saltyAnim.SetBool("isIdleWall", true);
                saltyAnim.SetBool("isFalling", false);
            }
            saltyAnim.SetFloat("PosX", movX);
            saltyAnim.SetFloat("PosZ", movZ);

        }

        if (rustyAgent.enabled)
            rustyAnim.SetBool("isRunning", true);
        

        lastRightTrigger = rightTriggerInput;
        lastLeftTrigger = leftTriggerInput;
    }

    private void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        Vector3 movHorizontal;
        Vector3 movVertical;
        bool isRunning = false;
        bool rustyIsRunning = false;

        saltyAnim.SetBool("jumpActivated", false);

        if (isSalty)
        {
            saltyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            saltyRig.interpolation = RigidbodyInterpolation.Interpolate;

            if ((rustyOnPlatform || rustyIsFalling) && !inRustyHands)
            {
                rustyRig.interpolation = RigidbodyInterpolation.Interpolate;
                rustyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                if (rustyOnPlatform)
                    rustyRig.velocity = new Vector3(0f, rustyRig.velocity.y, 0f);

            }
            else
            {
                rustyRig.interpolation = RigidbodyInterpolation.None;
                rustyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }

            
            if (!rustyIsFalling && !rustyOnPlatform && !inRustyHands)
            {
                rustyAgent.enabled = true;
            }
        }
        else
        {
            
            rustyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rustyRig.interpolation = RigidbodyInterpolation.Interpolate;

            if((saltyOnPlatform || saltyIsFalling) && !inRustyHands)
            {
                saltyRig.interpolation = RigidbodyInterpolation.Interpolate;
                saltyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                if(saltyOnPlatform)
                    saltyRig.velocity = new Vector3(0f, saltyRig.velocity.y, 0f);

            }
            else
            {
                saltyRig.interpolation = RigidbodyInterpolation.None;
                saltyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }

            if (!saltyIsFalling && !saltyOnPlatform && !inRustyHands)
            {
                saltyAgent.enabled = true;
            }
        }

        if (!isClimbing && !inRustyHands && !throwActivated || (jumpActivated && enteredLedgeGrab))
        {
            // update current character position and rotation
            currentCharacter.position = camPivotPlaceHolder.position;
            currentCharacter.rotation = cameraPivot.rotation;

            if (isSalty)
            {
                // update isKinematic on rigid bodies

                if (!rustyOnPlatform && !rustyIsFalling)
                {
                    saltyRig.isKinematic = false;
                    rustyRig.isKinematic = true;
                }
                else
                {
                    saltyRig.isKinematic = false;
                    rustyRig.isKinematic = false;
                }
            }
            else
            {
                // update isKinematic on rigid bodies

                if (!saltyOnPlatform && !saltyIsFalling)
                {
                    rustyRig.isKinematic = false;
                    saltyRig.isKinematic = true;
                }
                else
                {
                    rustyRig.isKinematic = false;
                    saltyRig.isKinematic = false;
                }
            }

            // zero out X and Z rotation for movement vectors
            currentCharacter.rotation = Quaternion.Euler(0, currentCharacter.eulerAngles.y, 0);

            // set movement vectors
            movVertical = currentCharacter.forward * movZ;
            movHorizontal = currentCharacter.right * movX;

            // set velocity vector direction
            velocity = (movVertical + movHorizontal).normalized;

            if (isBeingThrown && !saltyIsFalling)
            {
                isBeingThrown = false;
                saltyThrowVFX.SetActive(false);
            }

            int playerMask = LayerMask.GetMask("Salty") | LayerMask.GetMask("Rusty");

            if (!isClimbing && !Physics.Raycast(salty.transform.position + Vector3.up*0.1f, Vector3.down, 0.4f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                if(isSalty)
                    velocity *= fallingSpeed;

                // update falling / throw bools
                saltyIsFalling = true;
                saltyAnim.SetBool("isFalling", true);
            }
            else
            {
                if (isAiming && isSalty)
                {
                    velocity *= 0f;
                }
                else if(isSalty)
                {
                    velocity *= runningSpeed;
                }
                

                saltyIsFalling = false;
                saltyAnim.SetBool("isFalling", false);
            }

            // jump code
            if (!rustyIsClimbing && !Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f, Vector3.down, 0.4f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                if(!isSalty)
                    velocity *= fallingSpeed;

                rustyIsFalling = true;
                rustyAnim.SetBool("isFalling", true);
              
            }
            else
            {
                if(!isSalty)
                    velocity *= runningSpeed;

                rustyAnim.SetBool("isFalling", false);
                rustyIsFalling = false;
            }
            
            if(jumpActivated && isSalty && !saltyIsFalling && (!isClimbing || enteredLedgeGrab))
            {
                if(enteredLedgeGrab)
                {
                    UpdateSaltyAfterClimb();
                }
                saltyJumpSound.Stop();
                saltyJumpSound.Play();
                saltyRig.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                saltyAnim.SetBool("jumpActivated", true);
                jumpActivated = false;
            }
            else if (jumpActivated && !isSalty && !rustyIsFalling && !rustyIsClimbing)
            {
                rustyRig.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                jumpActivated = false;
                rustyJumpSound.Stop();
                rustyJumpSound.Play();
            }

            if (saltyIsFalling && saltyRig.velocity.y < 0f)
            {
                saltyRig.AddForce(Vector3.down * 15, ForceMode.Force);
            }
            if (rustyIsFalling && rustyRig.velocity.y < 0f)
            {
                rustyRig.AddForce(Vector3.down * 15, ForceMode.Force);
            }

            

            // movement code
            if (!isBeingThrown && isSalty && (movZ != 0 || movX != 0) && !Physics.Raycast(salty.transform.position + Vector3.up*0.4f, saltyModelTrans.forward, 0.6f, ~LayerMask.GetMask("Salty"), QueryTriggerInteraction.Ignore))
            {
                // is salty and can move (nothing in front of salty)

                // calculate new position and move to it
                Vector3 newPos = saltyRig.position + velocity * Time.fixedDeltaTime;
                Vector3 actualVelocity = new Vector3(velocity.x, saltyRig.velocity.y, velocity.z);

                saltyRig.velocity = actualVelocity;

                //saltyRig.MovePosition(newPos);
                isRunning = true;
                
            }
            else if (!isBeingThrown && isSalty && movZ == 0 && movX == 0)
            {
                Vector3 actualVelocity = new Vector3(0f, saltyRig.velocity.y, 0f);

                saltyRig.velocity = actualVelocity;
                isRunning = true;
            }
            else if(isBeingThrown)
            {
                saltyRig.velocity = saltyRig.velocity;
                saltyThrowVFX.SetActive(true);
            }
            else if (!isSalty && !inRustyHands)
            {
                // update movement running value in animators
                rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
                saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(saltyAgent.velocity.magnitude), 0f, 1f));

                //Debug.DrawRay(rusty.transform.position + Vector3.up * 0.1f, rustyModelTans.forward, Color.red, 20f, false);

                RaycastHit blockHit;

                

                if(Animator.StringToHash("punch_1") == rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash)
                {
                    //Debug.Log("Yoooooo");
                    inPunchAnimation = true;
                }
                else
                {
                    inPunchAnimation = false;
                }

                if (Animator.StringToHash("punch_2") == rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash)
                {
                    //Debug.Log("Yoooooo");
                    inPunchAnimationTwo = true;
                }
                else
                {
                    inPunchAnimationTwo = false;
                }

                if (!inPunchAnimation && !inPunchAnimationTwo && velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f + -rustyModelTans.forward * 0.1f, rustyModelTans.forward, 0.8f, LayerMask.GetMask("Ladders")))
                {
                    if (rustyIsFalling && !Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f, Vector3.down, 0.6f, ~playerMask, QueryTriggerInteraction.Ignore))
                    {

                    }
                    else
                    {
                        if(rustyAnim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("ladder"))
                        {
                            rustyIsClimbing = true;
                            rustyRig.useGravity = false;

                            rustyRig.velocity = Vector3.up * rustyAnim.GetCurrentAnimatorStateInfo(0).speed / 2f;
                        }
                        // ladder in front so climb
                        

                        pushCollider.gameObject.SetActive(false);

                        // set velocity to up direction and turn off gravity
                        velocity = Vector3.up;

                        
                        // update anim bools
                        rustyAnim.SetBool("isPushing", false);
                        rustyAnim.SetBool("isClimbing", true);
                    }
                    
                }
                else if (!inPunchAnimation && !inPunchAnimationTwo && velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f + -rustyModelTans.forward * 0.1f, rustyModelTans.forward, out blockHit, 1.2f, LayerMask.GetMask("PushableBlocks")))
                {
                    // pushable block in front so push block

                    rustyIsClimbing = false;

                    // turn gravity back on
                    rustyRig.useGravity = true;

                    pushCollider.gameObject.SetActive(true);

                    // update velocity to direction towards block
                    velocity = (-blockHit.normal) * pushSpeed;

                    // update anim bools
                    rustyAnim.SetBool("isPushing", true);
                    rustyAnim.SetBool("isClimbing", false);

                    // move block
                    Rigidbody blockRig = blockHit.transform.gameObject.GetComponent<Rigidbody>();
                    blockRig.velocity = new Vector3(velocity.x, blockRig.velocity.y, velocity.z);              
                }
                else if(inPunchAnimation || inPunchAnimationTwo)
                {
                    if(punchActivated)
                    {


                        rustyRig.velocity = Vector3.zero;
                        punchActivated = false;
                        rustyAnim.SetBool("punchActivated", false);
                        if (targetInRange)
                        {
                            //velocity = (meleeTargetPosition - rusty.transform.position).normalized * meleeSnapSpeed;
                            Vector3 dir = new Vector3(meleeTargetPosition.x - rusty.transform.position.x, 0, meleeTargetPosition.z - rusty.transform.position.z);
                            rustyRig.AddForce(dir.normalized * 3f, ForceMode.Impulse);
                        }
                    }
                    if(inPunchAnimationTwo && secondPunchActivated)
                    {
                        rustyRig.velocity = Vector3.zero;
                        secondPunchActivated = false;
                        rustyAnim.SetBool("secondPunchActivated", false);
                        if (targetInRange)
                        {
                            //velocity = (meleeTargetPosition - rusty.transform.position).normalized * meleeSnapSpeed;
                            Vector3 dir = new Vector3(meleeTargetPosition.x - rusty.transform.position.x, 0, meleeTargetPosition.z - rusty.transform.position.z);
                            rustyRig.AddForce(dir.normalized * 3f, ForceMode.Impulse);
                        }
                    }
                    pushCollider.gameObject.SetActive(false);

                    if(!targetInRange)
                    {
                        velocity = Vector3.zero;
                        rustyRig.velocity = Vector3.zero;
                    }
                    /*if (rustyRig.velocity.y > 0)
                        rustyRig.velocity = new Vector3(rustyRig.velocity.x, 0, rustyRig.velocity.z);*/
                    // update anim bools and turn gravity back on
                    rustyAnim.SetBool("isPushing", false);
                    rustyRig.useGravity = true;
                    rustyAnim.SetBool("isClimbing", false);
                }
                else
                {
                    // not pushing or climbing
                    rustyIsClimbing = false;

                    pushCollider.gameObject.SetActive(false);

                    // update anim bools and turn gravity back on
                    rustyAnim.SetBool("isPushing", false);
                    rustyRig.useGravity = true;
                    rustyAnim.SetBool("isClimbing", false);
                }

                // rusty movement code
                if ((movZ != 0 || movX != 0) && !inPunchAnimation && !inPunchAnimationTwo && !inRustyHands && !Physics.Raycast(rusty.transform.position + Vector3.up * 0.4f, rustyModelTans.forward, 0.6f, ~LayerMask.GetMask("Rusty"), QueryTriggerInteraction.Ignore))
                {
                    rustyIsRunning = true;
                    Vector3 actualVelocity;
                    actualVelocity = new Vector3(velocity.x, rustyRig.velocity.y, velocity.z);
                    rustyRig.velocity = actualVelocity;
                }
                else if (movZ == 0 && movX == 0 && !inPunchAnimation && !inPunchAnimationTwo)
                {
                    rustyIsRunning = true;
                    Vector3 actualVelocity;
                    actualVelocity = new Vector3(0f, rustyRig.velocity.y, 0f);
                    rustyRig.velocity = actualVelocity;
                }
                else if(inRustyHands)
                {
                    rustyRig.velocity = Vector3.zero;
                }
            }
            rustyAnim.SetBool("isRunning", rustyIsRunning);
            enteredLedgeGrab = false;
        }
        else if (isSalty && isClimbing) // isClimbing
        {
            if (saltyLeftHandInTrigger || saltyRightHandInTrigger)
            {
                enteredLedgeGrab = true;
            }

            


            // update rusty anim velocity
            rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(rustyAgent.velocity.magnitude), 0f, 1f));

            if (climbStarted)
            {
                // set position to raycast hit point + the wall offset
                posToMove = startHit.point + (startHit.normal * wallOffset);

                // need to lerp and reset climbStarted
                //isLerping = true;
                climbStarted = false;
            }

            // set movement vectors based on input and current wall attached to
            movVertical = Vector3.Cross(currentWallDir, Vector3.Cross(currentWallDir, up.normalized)) * -movZ;
            movHorizontal = Vector3.Cross(currentWallDir, up.normalized) * -movX;

            // set velocity vector
            velocity = (movVertical + movHorizontal).normalized * climbingSpeed;

            if (enteredLedgeGrab)
            {
                velocity = new Vector3(velocity.x, 0f, velocity.z);
            }
            else
            {

            }

            if (velocity != Vector3.zero) // is moving on wall
            {
                // update climb movement anim bools
                saltyAnim.SetBool("isIdleWall", false);
                saltyAnim.SetBool("isFalling", false);

                // raycast from newPos towards where salty is facing

                Debug.DrawRay(salty.transform.position + velocity / 2f, currentWallDir, Color.cyan);
                if (Physics.Raycast(salty.transform.position + velocity/2f, currentWallDir, out RaycastHit hit, turnThresh, LayerMask.GetMask("Climable Walls")))
                {
                    currentWallDir = -1 * hit.normal;
                }
                else
                {
                    velocity = Vector3.zero;
                }

            }
            else // hanging on the wall
            {
                // update climb movement anim bools
                saltyAnim.SetBool("isIdleWall", true);
                saltyAnim.SetBool("isFalling", false);
            }

            if (isLerping) // position to move is on a new wall so lerp position
            {
                Vector3 tempPos = saltyModelTrans.position;

                // update position to position to move
                salty.transform.position = (posToMove);

                // lerp model rotation to position to move
                saltyModelTrans.position = Vector3.Lerp(tempPos, posToMove, 0.02f);

                // if model position is at lerp position, then don't need to keep lerping
                if (Vector3.Distance(saltyModelTrans.position, posToMove) < 0.02f)
                {
                    saltyModelTrans.position = posToMove;
                    isLerping = false;
                }

            }
            else // position to move is on same wall so just update the new position
            {
                //saltyRig.MovePosition(posToMove);
                Debug.DrawRay(salty.transform.position, velocity, Color.red);
                saltyRig.velocity = velocity;
            }

            // set rotation for salty model placeholder
            saltyModelPlaceHolder.rotation = Quaternion.LookRotation(currentWallDir, up);

            // **** CURRENTLY NOT ROTATING CHARACTER TRANS WHEN CLIMBING ****
            // Quaternion newYRot = Quaternion.Euler(n.x, n.y, n.z);
            // salty.transform.rotation = Quaternion.Lerp(salty.transform.rotation, newYRot, 0.005f);

            // lerp the rotation of the model and collider to placeHolder rotation (new rotation)
            saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, saltyModelPlaceHolder.rotation, 0.009f);
            sCollider.rotation = Quaternion.Lerp(saltyModelTrans.rotation, saltyModelPlaceHolder.rotation, 0.009f);
        }
        else if (throwActivated)
        {
            rustyRig.constraints = RigidbodyConstraints.FreezeRotation;
            enteredLedgeGrab = false;
            // turn isKinematic off for salty and gravity back on
            saltyRig.isKinematic = false;
            saltyRig.useGravity = true;

            Debug.DrawRay(salty.transform.position, (endThrowPoint.position - rusty.transform.position).normalized + Vector3.up, Color.cyan, 10f);

            //callback

            // add a force towards the end position and upwards
            saltyRig.AddForce(((endThrowPoint.position - rusty.transform.position).normalized + Vector3.up) * throwForceMagnitude, ForceMode.Impulse);

            // turn collider back on and reset throwActivated
            
            throwActivated = false;

            StartCoroutine(ColliderDelay(0.5f));

            // switch to salty
            isSalty = true;
            ButtonScript.isSalty = true;

            StartLerping();

            // update parent of cam pivot placeholder
            camPivotPlaceHolder.SetParent(saltyPos);

            // update Nav Agents
            rustyAgent.enabled = true;
            saltyAgent.enabled = false;

            // update bools
            isBeingThrown = true;
            saltyIsFalling = true;
            saltyAnim.SetBool("isFalling", true);
            isSwitching = true;
        }
        movX = 0f;
        movZ = 0f;
        jumpActivated = false;
        saltyAnim.SetBool("isRunning", isRunning);
    }

    private IEnumerator ColliderDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sCollider.gameObject.SetActive(true);
    }

    private void UpdateSaltyAfterClimb()
    {
        // update falling / climbing bools
        saltyIsFalling = true;
        isClimbing = false;
        saltyAnim.SetBool("isFalling", true);
        saltyAnim.SetBool("isClimbing", false);

        // turn gravity back on and reset velocity (for safe measures)
        saltyRig.useGravity = true;
        saltyRig.velocity = Vector3.zero;

        // turn collider back on and climb collider off
        sCollider.gameObject.SetActive(true);
        sCollider.GetComponent<CapsuleCollider>().isTrigger = false;

        climbCollider.gameObject.SetActive(false);

        // *** may be unneccessary now (not sure) *** update exit rotation to match Y euler of current camera Pivot rotation
        //exitRotationTarget = Quaternion.Euler(new Vector3(nonClimbingRot.x, cameraPivot.localRotation.eulerAngles.y, nonClimbingRot.z));
    }

    public void PunchOneEnded()
    {
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            // rotate camera left / right
             saltyModelTrans.rotation = Quaternion.Euler(saltyModelTrans.rotation.eulerAngles.x, cameraPivot.rotation.eulerAngles.y, saltyModelTrans.rotation.eulerAngles.z);
        }

        mouseX = 0;
    }
}