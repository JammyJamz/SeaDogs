using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToHubScript : MonoBehaviour
{
    public 
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
        if(Salty_Rusty_Controller.isSalty && collider.tag == "saltyCollider" || !Salty_Rusty_Controller.isSalty && collider.tag == "rustyCollider")
        {
            CanvasData.levelLoader.LoadLevel(1);
        }
        
        Debug.Log("Heyy");
    }
}
