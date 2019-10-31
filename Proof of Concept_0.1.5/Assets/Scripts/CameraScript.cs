using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSens = 15f;
    public Transform pivotPos;
    public Transform cameraPivot;
    public Transform camPivotPlaceHolder;
    private float mouseX;
    private float mouseY;
    
    // Start is called before the first frame update
    void Start()
    {
        mouseX = 0f;
        mouseY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // get camera input
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        mouseX += Input.GetAxis("Controller X") * Time.deltaTime * mouseSens;

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;
    }

    private void LateUpdate()
    {
        
        // rotate camera left / right
        cameraPivot.Rotate(new Vector3(0, mouseX, 0), Space.World);

        // invert Y value
        mouseY *= -1;

        // code to lock up / down camera movement;
        float angle = cameraPivot.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        if (mouseY < 0 && (angle + mouseY < -32))
        {
            mouseY = 0;
        }
        else if (mouseY > 0 && angle + mouseY > 45)
        {
            mouseY = 0;
        }

        // rotate camera up / down
        cameraPivot.Rotate(new Vector3(mouseY, 0, 0));

        // reset mouse vars
        mouseY = 0;
        mouseX = 0;

        pivotPos.position = camPivotPlaceHolder.position;
        
    }
}
