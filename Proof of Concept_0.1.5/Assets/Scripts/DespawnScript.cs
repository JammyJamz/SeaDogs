using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
