using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlunderbussScript : MonoBehaviour
{
    public Transform camPivot;

    public GameObject BlunderVFX;

    public static bool shot;

    private float timer;
    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        BlunderVFX.SetActive(false);
        shot = false;
        timer = 0f;
        activated = false;
    }

    void Awake()
    {
        BlunderVFX.SetActive(false);
        shot = false;
        timer = 0f;
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(camPivot.rotation.eulerAngles.x, camPivot.rotation.eulerAngles.y, camPivot.rotation.eulerAngles.z);

        if(shot)
        {
            BlunderVFX.SetActive(true);
            activated = true;
            shot = false;
        }

        if(activated)
        {
            timer += Time.deltaTime;

            if(timer >= 0.4f)
            {
                timer = 0f;
                activated = false;
                BlunderVFX.SetActive(false);
            }
        }
    }
}
