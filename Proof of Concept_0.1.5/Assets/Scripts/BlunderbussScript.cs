using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlunderbussScript : MonoBehaviour
{
    public Transform camPivot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(camPivot.rotation.eulerAngles.x, camPivot.rotation.eulerAngles.y, camPivot.rotation.eulerAngles.z);
    }
}
