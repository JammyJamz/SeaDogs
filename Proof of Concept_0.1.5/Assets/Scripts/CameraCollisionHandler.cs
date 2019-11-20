using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CameraCollisionHandler
{

    public LayerMask camCollisionLayer;

    [HideInInspector]
    public bool colliding = false;

    [HideInInspector]
    public Vector3[] adjustedCameraClipPoints;

    [HideInInspector]
    public Vector3[] desiredCameraClipPoints;

    Camera camera;

    public void Initialize(Camera cam)
    {
        camera = cam;
        adjustedCameraClipPoints = new Vector3[5];
        desiredCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] array)
    {
        if (!camera)
            return;

        array = new Vector3[5];

        float z = camera.nearClipPlane;
        float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
        float y = x / camera.aspect;

        // top left
        array[0] = cameraPosition + (atRotation * new Vector3(-x, y, z));

        // top right
        array[1] = cameraPosition + (atRotation * new Vector3(x, y, z));

        // bottom left
        array[2] = cameraPosition + (atRotation * new Vector3(-x, -y, z));

        // bottom right
        array[3] = cameraPosition + (atRotation * new Vector3(x, -y, z));

        // camera position
        array[4] = cameraPosition - camera.transform.forward;
    }

    private bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
    {
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            float distance = Vector3.Distance(clipPoints[i], fromPosition);
            if(Physics.Raycast(ray, distance, camCollisionLayer, QueryTriggerInteraction.Ignore))
            {
                return true;
            }
        }

        return false;
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from)
    {
        float distance = -1;

        for (int i = 0; i < desiredCameraClipPoints.Length; i++)
        {
            Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
            float distanceRay = Vector3.Distance(desiredCameraClipPoints[i], from);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, distanceRay, camCollisionLayer))
            {
                if(distance == -1)
                {
                    distance = hit.distance;
                }
                else if(hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }
        }

        if (distance == -1)
            return 0f;
        else
            return distance;
    }

    public void CheckColliding(Vector3 targetPosition)
    {
        if(CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
        {
            colliding = true;
        }
        else
        {
            colliding = false;
        }
    }
}
