using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    public AudioSource saltyBlunderbuss;

    public GameObject bullet;

    public Animator saltyAnim;

    private float cooldown;

    private float timer;

    private bool startedAiming;

    private float lastXboxInput;

    // Start is called before the first frame update
    void Awake()
    {

        cooldown = 1f;
        timer = 0f;
        startedAiming = false;
    }

    private void Start()
    {
        lastXboxInput = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float xboxInput = Input.GetAxisRaw("Xbox Right Trigger");
        if (Salty_Rusty_Controller.isAiming && (saltyAnim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("aiming") /*|| saltyAnim.GetAnimatorTransitionInfo(0).userNameHash == Animator.StringToHash("aiming")*/))
        {
            if (!startedAiming)
            {
                startedAiming = true;
                timer = cooldown*2;
            }

            timer += Time.deltaTime;

            

            bool mouseDown = Input.GetMouseButtonDown(0);

            if(mouseDown)
            {
                xboxInput = 0;
            }

            bool triggerDown = false;

            if (lastXboxInput == 0 && xboxInput > 0)
                triggerDown = true;

            if(triggerDown)
            {
                Salty_Rusty_Controller.usingXboxController = true;
            }

            if ((mouseDown || triggerDown) && timer >= cooldown && saltyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("flinch") && saltyAnim.GetAnimatorTransitionInfo(0).userNameHash != Animator.StringToHash("flinch"))
            {
                BlunderbussScript.shot = true;
                saltyBlunderbuss.Play();

                Instantiate(bullet, transform.position, transform.rotation);
                timer = 0f;
                saltyAnim.SetBool("shot", true);
            }
        }
        else
        {
            startedAiming = false;
            timer = cooldown;
        }

        lastXboxInput = xboxInput;
    }

    private void LateUpdate()
    {
        saltyAnim.SetBool("shot", false);
    }
}
