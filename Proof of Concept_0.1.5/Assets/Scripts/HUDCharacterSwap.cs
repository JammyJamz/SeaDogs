using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCharacterSwap : MonoBehaviour
{

    public GameObject saltyImgSmall;
    public GameObject rustyImgSmall;
    public GameObject saltyImgBig;
    public GameObject rustyImgBig;
    private bool change = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            saltyImgSmall.SetActive(false);
            saltyImgBig.SetActive(true);
            rustyImgSmall.SetActive(true);
            rustyImgBig.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool xboxInput = Input.GetButtonDown("Xbox Character Switch");
        bool pcInput = Input.GetButtonDown("PC Character Switch");

        if (pcInput)
            xboxInput = false;

        if (!PauseMenu.GameIsPaused && (xboxInput || pcInput) && !Salty_Rusty_Controller.isSalty)
        {
            saltyImgSmall.SetActive(true);
            saltyImgBig.SetActive(false);
            rustyImgSmall.SetActive(false);
            rustyImgBig.SetActive(true);
        }
        else if (!PauseMenu.GameIsPaused && (xboxInput || pcInput) && Salty_Rusty_Controller.isSalty)
        {
            saltyImgSmall.SetActive(false);
            saltyImgBig.SetActive(true);
            rustyImgSmall.SetActive(true);
            rustyImgBig.SetActive(false);
        }
    }
}
