using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAIGuide : MonoBehaviour
{
    public float speed;
    public float width;
    public float height;
    public float length;

    private float x, y, z;
    private float counter;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        x = Mathf.Cos(counter) * width;
        y = Mathf.Sin(counter) * height;
        z = Mathf.Sin(counter) * length;

        transform.position = startPos + new Vector3(x, y, z);
    }
}
