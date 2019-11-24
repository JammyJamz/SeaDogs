using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderPoints : MonoBehaviour
{
    public Transform[] leftPoints;
    public Transform[] leftHandPoints;
    public Transform[] rightPoints;
    public Transform[] rightHandPoints;

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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Rusty") && collider.gameObject.tag == "rustyCollider")
        {
            IKControl.leftHandPoints = leftHandPoints;
            IKControl.rightHandPoints = rightHandPoints;
            IKControl.leftPoints = leftPoints;
            IKControl.rightPoints = rightPoints;
        }
    }
}
