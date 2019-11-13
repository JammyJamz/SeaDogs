using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparklesScript : MonoBehaviour
{

    public Transform coin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(coin.position.x, coin.position.y, coin.position.z);
    }
}
