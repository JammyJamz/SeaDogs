using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    private AudioSource saltyBlunderbuss;

    public GameObject bullet;

    public Animator saltyAnim;

    private bool mouseDown;

    private float cooldown;

    private float timer;

    private bool startedAiming;

    // Start is called before the first frame update
    void Awake()
    {
        mouseDown = false;

        cooldown = 1f;
        timer = 0f;
        startedAiming = false;
    }

    private void Start()
    {
        saltyBlunderbuss = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Salty_Rusty_Controller.isAiming && (saltyAnim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("aiming") /*|| saltyAnim.GetAnimatorTransitionInfo(0).userNameHash == Animator.StringToHash("aiming")*/))
        {
            if (!startedAiming)
            {
                startedAiming = true;
                timer = cooldown*2;
            }

            timer += Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && timer >= cooldown && saltyAnim.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("flinch") && saltyAnim.GetAnimatorTransitionInfo(0).userNameHash != Animator.StringToHash("flinch"))
            {
                BlunderbussScript.shot = true;
                saltyBlunderbuss.Play();
                mouseDown = true;
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
    }

    private void LateUpdate()
    {
        saltyAnim.SetBool("shot", false);
    }
}
