using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTailAnimator : MonoBehaviour
{
    public Animator SaltyAnim;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetFloat("PosZ", SaltyAnim.GetFloat("PosZ"));
        anim.SetFloat("PosX", SaltyAnim.GetFloat("PosZ"));

        anim.SetBool("isFalling", SaltyAnim.GetBool("isFalling"));
        anim.SetBool("isAiming", SaltyAnim.GetBool("isAiming"));

    }
}
