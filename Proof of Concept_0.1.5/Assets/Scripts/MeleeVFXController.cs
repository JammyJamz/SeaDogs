using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeVFXController : MonoBehaviour
{
    public GameObject meleeVFX;

    public bool isRightHand;

    public static bool rightHandHit;
    public static bool leftHandHit;



    // Start is called before the first frame update
    void Start()
    {
        meleeVFX.SetActive(false);
        if (isRightHand)
            rightHandHit = false;
        else
            leftHandHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRightHand)
        {
            if (!Salty_Rusty_Controller.inPunchAnimation)
            {
                meleeVFX.SetActive(false);
            }
            if (rightHandHit)
            {
                meleeVFX.SetActive(true);
                rightHandHit = false;
            }
        }
        else
        {
            if (!Salty_Rusty_Controller.inPunchAnimationTwo)
            {
                meleeVFX.SetActive(false);
            }
            if (leftHandHit)
            {
                meleeVFX.SetActive(true);
                leftHandHit = false;
            }
        }
    }
}
