using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSens = 15f;
    public Transform pivotPos;
    public Transform cameraPivot;
    public Transform camPivotPlaceHolder;

    public CameraCollisionHandler collisionHandler = new CameraCollisionHandler();


    private float mouseX;
    private float mouseY;

    public float maxCamOffset = -4.019f;

    public float camOffset = -4.019f;

    private Camera cam;

    private GameObject desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        mouseX = 0f;
        mouseY = 0f;

        cam = Camera.main;

        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camOffset);

        desiredPosition = GameObject.Instantiate(new GameObject(), cam.transform.position, cam.transform.rotation, cam.transform.parent);

        collisionHandler.Initialize(cam);
        collisionHandler.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(desiredPosition.transform.position, cam.transform.rotation, ref collisionHandler.desiredCameraClipPoints);
    }

    // Update is called once per frame
    void Update()
    {
        // get camera input
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        mouseX += Input.GetAxis("Controller Y") * Time.deltaTime * mouseSens;

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;
        mouseY += Input.GetAxis("Controller X") * Time.deltaTime * mouseSens;

        collisionHandler.UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(desiredPosition.transform.position, cam.transform.rotation, ref collisionHandler.desiredCameraClipPoints);

        for(int i = 0; i < 5; i++)
        {
            Debug.DrawLine(pivotPos.position, collisionHandler.desiredCameraClipPoints[i], Color.white);
            Debug.DrawLine(pivotPos.position, collisionHandler.adjustedCameraClipPoints[i], Color.green);
        }

        collisionHandler.CheckColliding(pivotPos.position);

        if(true)
            camOffset = -1 * collisionHandler.GetAdjustedDistanceWithRayFrom(pivotPos.position) + 0.5f;

        if(camOffset < maxCamOffset)
        {
            camOffset = maxCamOffset;
        }
    }

    private void LateUpdate()
    {
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

        if(collisionHandler.colliding)
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camOffset);
        }
        else
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, maxCamOffset);
        }

        pivotPos.position = camPivotPlaceHolder.position;
        
    }
}
