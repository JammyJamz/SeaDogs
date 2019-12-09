using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHealth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 200*Time.deltaTime, transform.eulerAngles.z);
    }
}
