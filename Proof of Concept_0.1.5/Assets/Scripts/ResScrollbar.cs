using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResScrollbar : MonoBehaviour
{
    Scrollbar sb;
    // Start is called before the first frame update
    bool  usingXboxController;
    void Start()
    {
        sb = GetComponent<Scrollbar>();
        usingXboxController = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (Input.anyKeyDown || Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                usingXboxController = false;
            }
        }

        if(usingXboxController)
        {
            sb.value = Mathf.Abs(1 - (1f / MainMenu.numOfReses) * (ResItem.index + 0.5f));
        }
    }
}
