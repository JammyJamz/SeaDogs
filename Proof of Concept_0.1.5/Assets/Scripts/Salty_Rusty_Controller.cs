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
    private bool isSalty, climbStarted, isFalling, isLerping, switchKeyDown, isSwitching, isClimbing, climbKeyDown, throwKeyDown, inRustyHands;
    private bool throwActivated;
    private bool isBeingThrown;
    private bool startSwitchVFX;

    private float vfxTimer;

    private void Start()
    {
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
        camPivotPlaceHolder.SetParent(saltyModelTrans);
        camPivotPlaceHolder.position = saltyPivotPos.position;
        // update Nav Agents
        rustyAgent.enabled = true;
        saltyAgent.enabled = false;
        rCollider.gameObject.SetActive(false);
        sCollider.gameObject.SetActive(true);

        nonClimbingRot = new Vector3 (cameraPivot.localRotation.eulerAngles.x, cameraPivot.localRotation.eulerAngles.y, cameraPivot.localRotation.eulerAngles.z);
        exitRotationTarget = Quaternion.Euler(nonClimbingRot.x, nonClimbingRot.y, nonClimbingRot.z);

        rustyRing.SetActive(false);
        saltyRing.SetActive(false);

        vfxTimer = 0f;
    }


    private void Update()
    {
        pivotPos.position = camPivotPlaceHolder.position;
        // reset vars
        climbKeyDown = false;

        // get movement input
        movZ = Input.GetAxisRaw("Vertical");
        movX = Input.GetAxisRaw("Horizontal");

        // get button inputs
        switchKeyDown = Input.GetKeyDown("e");
        climbKeyDown = Input.GetKeyDown("r");
        throwKeyDown = Input.GetKeyDown("v");

        // get camera input
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        mouseX += Input.GetAxis("Controller X") * Time.deltaTime * mouseSens;

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;

        // switch key pressed
        if (switchKeyDown && !isClimbing)
        {
            // salty is not climbing so allowed to switch

            if(isSalty)
            {
                // switch to rusty

                isSalty = false;
                ButtonScript.isSalty = false;

                // update parent of cam pivot placeholder to rusty's model
                camPivotPlaceHolder.SetParent(rustyModelTans);

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
                camPivotPlaceHolder.SetParent(saltyModelTrans);

                // update Nav Agents
                rustyAgent.enabled = true;
                saltyAgent.enabled = false;

                // update colliders
                rCollider.gameObject.SetActive(false);
                sCollider.gameObject.SetActive(true);
            }
            
            // update isSwitching bool
            isSwitching = true;

            // reset switchKeyDown
            switchKeyDown = false;
            startSwitchVFX = true;
            vfxTimer = 0f;
            saltyRing.SetActive(false);
            rustyRing.SetActive(false);
        }
        else if(climbKeyDown && isSalty) // climb key pressed
        {
            // isSalty so can start climb

            if(!isClimbing) // not currently climbing
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
        else if(throwKeyDown && !isSalty && !inRustyHands && inThrowTrigger)
        {
            // throw key was pressed by rusty in the throw trigger

            // update bools
            inRustyHands = true;
            sCollider.gameObject.SetActive(false);
            saltyAnim.SetBool("isReadyForThrow", true);

            // don't want salty moving on navmesh when in rusty's hands
            saltyAgent.enabled = false;
        }
        else if(throwKeyDown && !isSalty && inRustyHands)
        {
            // salty in rusty's hands and throw key was pressed so initiate throw

            // update bools
            throwActivated = true;
            inRustyHands = false;
        }
        if(!inRustyHands && !throwActivated)
        {
            // update destinations of NavMesh Agents
            if (isSalty)
            {
                // update destination of rusty AI
                rustyAgent.SetDestination(salty.transform.position);

            }
            else
            {
                // update destination of salty AI
                saltyAgent.SetDestination(rusty.transform.position);
            }
        }

        // reset keys
        throwKeyDown = false;
        climbKeyDown = false;
    }

    private void FixedUpdate()
    {
        Vector3 movVertical;
        Vector3 movHorizontal;

        // reset velocity
        velocity = Vector3.zero;

        // rotate camera left / right
        cameraPivot.Rotate(new Vector3(0, mouseX, 0), Space.World);
        
        // invert Y value
        mouseY *= -1;

        // code to lock up / down camera movement;
        float angle = cameraPivot.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        if (mouseY < 0 && (angle + mouseY < -32))
        {
            mouseY = 0;
        }
        else if (mouseY > 0 && angle + mouseY > 45)
        {
            mouseY = 0;
        }

        // rotate camera up / down
        cameraPivot.Rotate(new Vector3(mouseY, 0, 0));

        // reset mouse vars
        mouseY = 0;
        mouseX = 0;

        if(startSwitchVFX)
        {
            
            if(isSalty)
            {
                saltyRing.SetActive(true);
            }
            else
            {
                rustyRing.SetActive(true);
            }

            vfxTimer += Time.fixedDeltaTime;

            if(vfxTimer >= 1.5f)
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

            if (isSalty)
            {
                // lerp local position of place holder to salty's pivot position
                camPivotPlaceHolder.localPosition = Vector3.Lerp(camPivotPlaceHolder.localPosition, saltyPivotPos.localPosition, 0.025f);

                // if it's close eneough, turn off switching
                if (Vector3.Distance(cameraPivot.localPosition, saltyPivotPos.localPosition) < 0.0009f)
                {
                    cameraPivot.localPosition = saltyPivotPos.localPosition;
                    isSwitching = false;
                }
            }
            else
            {
                // lerp local position of place holder to rusty's pivot position
                camPivotPlaceHolder.localPosition = Vector3.Lerp(camPivotPlaceHolder.localPosition, rustyPivotPos.localPosition, 0.025f);

                // if it's close eneough, turn off switching
                if (Vector3.Distance(cameraPivot.localPosition, rustyPivotPos.localPosition) < 0.0009f)
                {
                    cameraPivot.localPosition = rustyPivotPos.localPosition;
                    isSwitching = false;
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

            // check rigid body velocity to determine if falling or not
            if(saltyRig.velocity.y < -1.5f)
            {
                // falling so multiply by falling speed
                velocity *= fallingSpeed;

                // update falling / throw bools
                isFalling = true;
                saltyAnim.SetBool("isFalling", true);
                isBeingThrown = false;
            }
            else if (!isBeingThrown)
            {
                // not falling and not being thrown so multiply by running speed
                velocity *= runningSpeed;
                saltyAnim.SetBool("isFalling", false);
            }

            // update anim bools for falling
            if(rustyRig.velocity.y < -1.5f)
            {
                rustyAnim.SetBool("isFalling", true);
            }
            else
            {
                rustyAnim.SetBool("isFalling", false);
            }

            // check to see if moving
            if (velocity != Vector3.zero)
            {
                // get new rotation based of movement vectors
                Quaternion newRot = Quaternion.LookRotation((movVertical + movHorizontal).normalized, Vector3.up);

                // lerp rotation of current character to new rotation
                if (isSalty)
                    saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, newRot, 0.05f);
                else
                    rustyModelTans.rotation = Quaternion.Lerp(rustyModelTans.rotation, newRot, 0.05f);
            }
            
            if(isSalty)
            {
                // lerps salty model and collider rotation back to vertical rotation since not climbing
                saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, Quaternion.Euler(0, saltyModelTrans.eulerAngles.y, 0), 0.02f);
                sCollider.rotation = Quaternion.Lerp(sCollider.rotation, Quaternion.Euler(0, saltyModelTrans.eulerAngles.y, 0), 0.02f);
            }


            // movement code
            if (isSalty && !Physics.Raycast(salty.transform.position + Vector3.up, saltyModelTrans.forward, 0.6f, ~LayerMask.GetMask("Salty"), QueryTriggerInteraction.Ignore))
            {
                // is salty and can move (nothing in front of salty)

                // calculate new position and move to it
                Vector3 newPos = saltyRig.position + velocity * Time.fixedDeltaTime;
                saltyRig.MovePosition(newPos);
            }
            else if (!isSalty)
            {
                // update movement running value in animators
                rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
                saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(saltyAgent.velocity.magnitude), 0f, 1f));

                //Debug.DrawRay(rusty.transform.position + Vector3.up * 0.1f, rustyModelTans.forward, Color.red, 20f, false);

                RaycastHit blockHit;

                //Debug.Log(Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f, rustyModelTans.forward, LayerMask.GetMask("Climable Walls")));

                if (velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up*0.1f + -rustyModelTans.forward*0.1f, rustyModelTans.forward, 0.8f, LayerMask.GetMask("Ladders")))
                {
                    // ladder in front so climb

                    // set velocity to up direction and turn off gravity
                    velocity = Vector3.up*1.5f;
                    rustyRig.useGravity = false;

                    // update anim bools
                    rustyAnim.SetBool("isPushing", false);
                    rustyAnim.SetBool("isClimbing", true);
                }
                else if (velocity != Vector3.zero && Physics.Raycast(rusty.transform.position + Vector3.up * 0.1f + -rustyModelTans.forward * 0.1f, rustyModelTans.forward, out blockHit, 1.9f, LayerMask.GetMask("PushableBlocks")))
                {
                    // pushable block in front so push block

                    // turn gravity back on
                    rustyRig.useGravity = true;

                    // update velocity to direction towards block
                    velocity = (-blockHit.normal) * pushSpeed;

                    // update anim bools
                    rustyAnim.SetBool("isPushing", true);
                    rustyAnim.SetBool("isClimbing", false);

                    // move block
                    blockHit.transform.position = blockHit.transform.position + velocity * Time.fixedDeltaTime;

                    // move rusty back so he doesn't clip through box
                    rusty.transform.position = Vector3.Lerp(rusty.transform.position, new Vector3(blockHit.transform.position.x, rusty.transform.position.y, blockHit.transform.position.z) + blockHit.normal * 1.8f, 0.05f);
                }
                else
                {
                    // not pushing or climbing

                    // update anim bools and turn gravity back on
                    rustyAnim.SetBool("isPushing", false);
                    rustyRig.useGravity = true;
                    rustyAnim.SetBool("isClimbing", false);
                }

                // calculate new position and move to it
                Vector3 newPos = rusty.transform.position + velocity * Time.fixedDeltaTime;
                rusty.transform.position = (newPos);
            }

            if (isSalty)
            {
                // update anim move vars
                saltyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(velocity.magnitude), 0f, 1f));
                rustyAnim.SetFloat("PosZ", Mathf.Clamp(Mathf.Abs(rustyAgent.velocity.magnitude), 0f, 1f));

                // AI model rotation
                Quaternion newRot = Quaternion.LookRotation(rusty.transform.forward, Vector3.up);
                rustyModelTans.rotation = Quaternion.Lerp(rustyModelTans.rotation, newRot, 0.05f);
            }
            else
            {
                // AI model rotation
                Quaternion newRot = Quaternion.LookRotation(salty.transform.forward, Vector3.up);
                saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, newRot, 0.05f);
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
                isLerping = true;
                climbStarted = false;
            }
            
            // set movement vectors based on input and current wall attached to
            movVertical = up * movZ;
            movHorizontal = Vector3.Cross(currentWallDir, up.normalized) * -movX;

            // set velocity vector
            velocity = (movVertical + movHorizontal).normalized * climbingSpeed;
            
            // get relative new position (changes to raycast hit + offset below)
            Vector3 newPos = (saltyRig.position + Vector3.up*0.25f) + velocity * Time.fixedDeltaTime;

            //Debug.DrawRay(newPos, saltyModelPlaceHolder.forward, Color.red, 20f, false);

            Vector3 dirToNextPos = Vector3.zero;

            if (velocity != Vector3.zero) // is moving on wall
            {
                // update climb movement anim bools
                saltyAnim.SetBool("isIdleWall", false);
                saltyAnim.SetBool("isFalling", false);

                // raycast from newPos towards where salty is facing
                if (Physics.Raycast(newPos, currentWallDir, out RaycastHit hit, turnThresh, LayerMask.GetMask("Climable Walls")))
                {

                    // check if the wall hit has a different normal than current wall
                    if (currentWallDir != -1 * hit.normal)
                    {
                        // it is so going to have to lerp position to new position on next wall
                        isLerping = true;
                    }

                    // update current wall direction to new wall
                    currentWallDir = -1 * hit.normal;

                    // update new position to hit point plus wall offset
                    newPos = (hit.point - Vector3.up*0.25f) + (hit.normal * wallOffset);

                    // update position to move to new position
                    posToMove = newPos;

                    // update direction to next position
                    dirToNextPos = newPos - saltyRig.position;
                }

                //Debug.DrawRay(newPos, saltyModelPlaceHolder.forward, Color.red, 20f, false);
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
                if(Vector3.Distance(saltyModelTrans.position, posToMove) < 0.02f)
                {
                    saltyModelTrans.position = posToMove;
                    isLerping = false;
                }

            }
            else // position to move is on same wall so just update the new position
            {
                saltyRig.MovePosition(posToMove);
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
        else if(inRustyHands)
        {
            // lerp salty's position to center of rusty's hands
            salty.transform.position = Vector3.Lerp(salty.transform.position, Vector3.Lerp(rustyRightHand.position, rustyLeftHand.position, 0.5f) + (rustyModelTans.forward*0.25f), 0.05f);

            // reset anim bool
            saltyAnim.SetBool("isReadyForThrow", false);

            // lerp salty's model rotation to rusty's rotation
            saltyModelTrans.rotation = Quaternion.Lerp(saltyModelTrans.rotation, rustyModelTans.rotation, 0.05f);
        }
        else if(throwActivated)
        {
            // turn isKinematic off for salty and gravity back on
            saltyRig.isKinematic = false;
            saltyRig.useGravity = true;

            // add a force towards the end position and upwards
            saltyRig.AddForce(((endThrowPoint.position - saltyRig.position).normalized + Vector3.up)*22f, ForceMode.Impulse);

            // turn collider back on and reset throwActivated
            sCollider.gameObject.SetActive(true);
            throwActivated = false;
            
            // switch to salty
            isSalty = true;
            ButtonScript.isSalty = true;

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
    }
}