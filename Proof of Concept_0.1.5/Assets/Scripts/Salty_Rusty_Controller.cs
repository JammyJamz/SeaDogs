using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Salty_Rusty_Controller : MonoBehaviour
{
    public static bool inThrowTrigger;
    public float runningSpeed = 15f;
    public float mouseSens = 15f;
    public float climbingSpeed = 15f;
    public float wallOffset = 0.5f;
    public float turnThresh = 0.5f;
    public float fallingSpeed = 1.5f;
    public float pushSpeed = 0.3f;

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

    public Transform endThrowPoint;

    public Transform rustyRightHand;
    public Transform rustyLeftHand;

    private Animator saltyAnim, rustyAnim;
    private Rigidbody saltyRig, rustyRig;
    private Transform saltyPos, rustyPos, currentCharacter;
    private float movX, movZ, mouseX, mouseY;
    private Vector3 fixRotation, posToMove, velocity, nonClimbingRot, currentWallDir, up;
    private RaycastHit startHit, lastHit;
    private Quaternion exitRotationTarget;
    private bool isSalty, climbStarted, isFalling, isLerping, switchKeyDown, isSwitching, isClimbing, climbKeyDown, throwKeyDown, inRustyHands, jumpKeyDown;
    private bool throwActivated;
    private bool isBeingThrown;
    private bool startSwitchVFX;

    private bool jumpActivated;

    private bool rustyIsClimbing;

    private float aiTimer;

    private float vfxTimer;

    private float rotInc;

    private float timeTakenDuringLerp = 0.25f;
    private float saltyTimeTakenDuringLerp = 0.25f;

    private Quaternion newRot;
    private Quaternion newRot2;

    //Whether we are currently interpolating or not
    private bool _isLerping;
    private bool _saltyIsLerping;

    //The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _saltyStartPos;
    private Vector3 _saltyEndPos;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;
    private float _saltyTimeStartedLerping;

    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = camPivotPlaceHolder.localPosition;
        _endPosition = rustyPivotPos.localPosition;
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
        Application.targetFrameRate = 300;
        // initialization of variables
        currentCharacter = (new GameObject()).transform;
        rustyRig = rusty.GetComponent<Rigidbody>();
        saltyRig = salty.GetComponent<Rigidbody>();

        rustyAnim = rusty.GetComponent<Animator>();
        saltyAnim = salty.GetComponent<Animator>();

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
        camPivotPlaceHolder.position = pivotPos.position;
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
        camPivotPlaceHolder.position = saltyPivotPos.position;
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
        _saltyIsLerping = false;

        newRot = saltyModelTrans.rotation;
        newRot2 = saltyModelTrans.rotation;

        Vector3 tempVert = salty.transform.forward*1;
        Vector3 tempHor = (salty.transform.right)*1;

        Quaternion rot = Quaternion.LookRotation((tempVert + tempHor).normalized, Vector3.up);

        rotInc = (salty.transform.rotation.eulerAngles.y - rot.eulerAngles.y)/2;
    }


    private void Update()
    {
        // reset vars
        climbKeyDown = false;

        // get movement input
        movZ = Input.GetAxisRaw("Vertical");
        movX = Input.GetAxisRaw("Horizontal");

        // get button inputs
        switchKeyDown = Input.GetKeyDown("e");
        climbKeyDown = Input.GetKeyDown("r");
        throwKeyDown = Input.GetKeyDown("v");
        jumpKeyDown = Input.GetKeyDown("space");

        // get camera input
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        mouseX += Input.GetAxis("Controller X") * Time.deltaTime * mouseSens;

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;

        if(jumpKeyDown)
        {
            jumpActivated = true;
        }

        // switch key pressed
        if (switchKeyDown && !isClimbing && !inRustyHands)
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
                saltyAgent.enabled = true;
                rustyAgent.enabled = false;
                
                // update colliders
                sCollider.gameObject.SetActive(false);
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
                rustyAgent.enabled = true;
                saltyAgent.enabled = false;

                // update colliders
                rCollider.gameObject.SetActive(false);
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
        else if (climbKeyDown && isSalty) // climb key pressed
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
                    if (isFalling)
                    {
                        // update falling bools
                        saltyAnim.SetTrigger("grabbedWallInAir");

                        // grabbed a wall so no longer falling
                        isFalling = false;
                        saltyAnim.SetBool("isFalling", false);
                    }

                    // save current direction to the wall that was hit
                    currentWallDir = hit.normal * -1;

                    // turn gravity off for salty
                    saltyRig.useGravity = false;

                    // turn salty collider off, reset velocity, turn climb collider on (so does not go through the floor)
                    sCollider.gameObject.SetActive(false);
                    saltyRig.velocity = Vector3.zero;
                    climbCollider.gameObject.SetActive(true);
                }
            }
            else // is climbing when pressed, so need to drop off wall
            {
                // update falling / climbing bools
                isFalling = true;
                isClimbing = false;
                saltyAnim.SetBool("isFalling", true);
                saltyAnim.SetBool("isClimbing", false);

                // turn gravity back on and reset velocity (for safe measures)
                saltyRig.useGravity = true;
                saltyRig.velocity = Vector3.zero;

                // turn collider back on and climb collider off
                sCollider.gameObject.SetActive(true);
                climbCollider.gameObject.SetActive(false);

                // *** may be unneccessary now (not sure) *** update exit rotation to match Y euler of current camera Pivot rotation
                //exitRotationTarget = Quaternion.Euler(new Vector3(nonClimbingRot.x, cameraPivot.localRotation.eulerAngles.y, nonClimbingRot.z));
            }
        }
        else if (throwKeyDown && !isSalty && !inRustyHands && inThrowTrigger)
        {
            // throw key was pressed by rusty in the throw trigger

            // update bools
            inRustyHands = true;
            sCollider.gameObject.SetActive(false);
            saltyAnim.SetBool("isReadyForThrow", true);

            StartLerpingSalty(salty.transform.position, Vector3.Lerp(rustyRightHand.position, rustyLeftHand.position, 0.5f) + (rustyModelTans.forward * 0.25f));

            // don't want salty moving on navmesh when in rusty's hands
            saltyAgent.enabled = false;
        }
        else if (throwKeyDown && !isSalty && inRustyHands)
        {
            // salty in rusty's hands and throw key was pressed so initiate throw

            // update bools
            throwActivated = true;
            inRustyHands = false;
        }

        // reset keys
        throwKeyDown = false;
        climbKeyDown = false;
        jumpKeyDown = false;

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
            if (velocity != Vector3.zero)
            {
                // get new rotation based of movement vectors
                newRot = Quaternion.LookRotation((movVertical + movHorizontal).normalized, Vector3.up);
                newRot2 = Quaternion.Euler(pushCollider.eulerAngles.x, newRot.eulerAngles.y, pushCollider.eulerAngles.z);

                // lerp rotation of current character to new rotation
                if(isSalty)
                {
                    saltyModelTrans.rotation = Quaternion.Slerp(saltyModelTrans.rotation, newRot, 7 * Time.deltaTime);
                }
                if(!isSalty)
                {
                    rustyModelTans.rotation = Quaternion.Slerp(rustyModelTans.rotation, newRot, 7 * Time.deltaTime);
                    pushCollider.rotation = Quaternion.Slerp(pushCollider.rotation, newRot2, 7 * Time.deltaTime);
                }
            }

            if (isSalty)
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
                        rustyAgent.destination = (saltyRig.position);
                    }
                    else
                    {
                        // update destination of salty AI
                        saltyAgent.SetDestination(rustyRig.position);
                    }
                }
                aiTimer = 0f;
            }

            if (isSalty)
            {
                // update anim move vars
                saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
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

            // reset anim bool
            saltyAnim.SetBool("isReadyForThrow", false);

            // lerp salty's model rotation to rusty's rotation
            saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, rustyModelTans.rotation, 5f * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        Vector3 movHorizontal;
        Vector3 movVertical;
        
        if(isSalty)
        {
            rustyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            saltyRig.collisionDetectionMode = CollisionDetectionMode.Continuous;
            saltyRig.interpolation = RigidbodyInterpolation.Interpolate;
            rustyRig.interpolation = RigidbodyInterpolation.None;
        }
        else
        {
            saltyRig.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rustyRig.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rustyRig.interpolation = RigidbodyInterpolation.Interpolate;
            saltyRig.interpolation = RigidbodyInterpolation.None;
        }

        if (!isClimbing && !inRustyHands && !throwActivated)
        {
            // update current character position and rotation
            currentCharacter.position = camPivotPlaceHolder.position;
            currentCharacter.rotation = cameraPivot.rotation;

            if (isSalty)
            {
                // update isKinematic on rigid bodies
                rustyRig.isKinematic = true;
                saltyRig.isKinematic = false;
            }
            else
            {
                // update isKinematic on rigid bodies
                rustyRig.isKinematic = false;
                saltyRig.isKinematic = true;
            }

            // zero out X and Z rotation for movement vectors
            currentCharacter.rotation = Quaternion.Euler(0, currentCharacter.eulerAngles.y, 0);

            // set movement vectors
            movVertical = currentCharacter.forward * movZ;
            movHorizontal = currentCharacter.right * movX;

            // set velocity vector direction
            velocity = (movVertical + movHorizontal).normalized;

            if (isBeingThrown && !isFalling)
            {
                isBeingThrown = false;
            }

            int playerMask = LayerMask.GetMask("Salty") | LayerMask.GetMask("Rusty");

            if (isSalty && !Physics.Raycast(salty.transform.position + Vector3.up*0.1f, Vector3.down, 0.4f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                velocity *= fallingSpeed;

                // update falling / throw bools
                isFalling = true;
                saltyAnim.SetBool("isFalling", true);
            }
            else if(isSalty)
            {
                velocity *= runningSpeed;
                isFalling = false;
                saltyAnim.SetBool("isFalling", false);
            }

            // jump code
            if (!isSalty && !rustyIsClimbing && !Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f, Vector3.down, 0.4f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                velocity *= fallingSpeed;
                isFalling = true;
                rustyAnim.SetBool("isFalling", true);
            }
            else if (!isSalty)
            {
                velocity *= runningSpeed;
                rustyAnim.SetBool("isFalling", false);
                isFalling = false;
            }

            if(jumpActivated && isSalty && !isFalling)
            {
                saltyRig.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                jumpActivated = false;
            }
            else if(jumpActivated && !isSalty && !isFalling)
            {
                rustyRig.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                jumpActivated = false;
            }

            if (isSalty && isFalling && saltyRig.velocity.y < 0f)
            {
                saltyRig.AddForce(Vector3.down * 15, ForceMode.Force);
            }
            else if (!isSalty && isFalling && rustyRig.velocity.y < 0f)
            {
                rustyRig.AddForce(Vector3.down * 15, ForceMode.Force);
            }

            

            // movement code
            if (!isBeingThrown && isSalty && (movZ != 0 || movX != 0) /*&& !Physics.Raycast(salty.transform.position + Vector3.up, saltyModelTrans.forward, 0.6f, ~LayerMask.GetMask("Salty"), QueryTriggerInteraction.Ignore)*/)
            {
                // is salty and can move (nothing in front of salty)

                // calculate new position and move to it
                Vector3 newPos = saltyRig.position + velocity * Time.fixedDeltaTime;
                Vector3 actualVelocity = new Vector3(velocity.x, saltyRig.velocity.y, velocity.z);

                saltyRig.velocity = actualVelocity;
                //saltyRig.MovePosition(newPos);
            }
            else if (!isBeingThrown && isSalty && movZ == 0 && movX == 0)
            {
                Vector3 actualVelocity = new Vector3(0f, saltyRig.velocity.y, 0f);
                saltyRig.velocity = actualVelocity;
            }
            else if(isBeingThrown)
            {
                saltyRig.velocity = saltyRig.velocity;
            }
            else if (!isSalty)
            {
                // update movement running value in animators
                rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
                saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(saltyAgent.velocity.magnitude), 0f, 1f));

                //Debug.DrawRay(rusty.transform.position + Vector3.up * 0.1f, rustyModelTans.forward, Color.red, 20f, false);

                RaycastHit blockHit;

                if (velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f + -rustyModelTans.forward * 0.1f, rustyModelTans.forward, 0.8f, LayerMask.GetMask("Ladders")))
                {
                    // ladder in front so climb
                    rustyIsClimbing = true;

                    pushCollider.gameObject.SetActive(false);

                    // set velocity to up direction and turn off gravity
                    velocity = Vector3.up;
                    
                    rustyRig.useGravity = false;

                    rustyRig.velocity = Vector3.up * climbingSpeed;
                    // update anim bools
                    rustyAnim.SetBool("isPushing", false);
                    rustyAnim.SetBool("isClimbing", true);
                }
                else if (velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f + -rustyModelTans.forward * 0.1f, rustyModelTans.forward, out blockHit, 1.2f, LayerMask.GetMask("PushableBlocks")))
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
                if (movZ != 0 || movX != 0)
                {
                    Vector3 actualVelocity;
                    actualVelocity = new Vector3(velocity.x, rustyRig.velocity.y, velocity.z);
                    rustyRig.velocity = actualVelocity;
                }
                else if (movZ == 0 && movX == 0)
                {
                    Vector3 actualVelocity;
                    actualVelocity = new Vector3(0f, rustyRig.velocity.y, 0f);
                    rustyRig.velocity = actualVelocity;
                }
            }
        }
        else if (isSalty && isClimbing) // isClimbing
        {
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
            // turn isKinematic off for salty and gravity back on
            saltyRig.isKinematic = false;
            saltyRig.useGravity = true;

            Debug.DrawRay(salty.transform.position, (endThrowPoint.position - rusty.transform.position).normalized + Vector3.up, Color.cyan, 10f);

            // add a force towards the end position and upwards
            saltyRig.AddForce(((endThrowPoint.position - rusty.transform.position).normalized + Vector3.up) * 11f, ForceMode.Impulse);

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
            isFalling = true;
            saltyAnim.SetBool("isFalling", true);
            isSwitching = true;
        }
        movX = 0f;
        movZ = 0f;
    }

    private IEnumerator ColliderDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sCollider.gameObject.SetActive(true);
    }
}