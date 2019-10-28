using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Salty"))
        {
            PauseMenu.gameWon = true;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty"))
        {
            PauseMenu.gameWon = true;
        }
    }
}
