using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDetection : MonoBehaviour
{
    public int hp = 150;
    public GameObject rootObj;

    private bool leftHandHit;

    public GameObject particleEffect;

    public ParticleSystem ps;

    private bool inPunchAnimation;

    private bool hitInAnimAlready;

    // Start is called before the first frame update
    void Start()
    {

        leftHandHit = false;
        inPunchAnimation = false;

        hitInAnimAlready = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.isPlaying)
        {

        }

        if(Salty_Rusty_Controller.inPunchAnimation)
        {
            if(inPunchAnimation == false)
            {
                inPunchAnimation = true;
            }
        }
        else
        {
            inPunchAnimation = false;
            hitInAnimAlready = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((this.gameObject.layer == 0 && collider.gameObject.tag == "rustyRightHand" && Salty_Rusty_Controller.inPunchAnimation && !hitInAnimAlready) || collider.gameObject.tag == "Bullet")
        {
            if (!hitInAnimAlready)
                hitInAnimAlready = true;
            Debug.Log(collider.name);
            hp = hp - 50;

            if(hp == 0)
            {
                //Debug.Log("setting false...");
                Salty_Rusty_Controller.targetInRange = false;
                particleEffect.SetActive(true);
                GameObject.Destroy(transform.parent.gameObject.GetComponent<BoxCollider>());
                GameObject.Destroy(transform.parent.gameObject, 0.2f);

                GameObject.Destroy(transform.root.gameObject.GetComponent<Rigidbody>());

                GameObject.Destroy(rootObj, 1f);

                
            }

            if (collider.gameObject.tag == "Bullet")
            {
                GameObject.Destroy(collider.gameObject);
            }
            else
            {
                MeleeVFXController.rightHandHit = true;
            }
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (this.gameObject.layer == 0 && !leftHandHit && collider.gameObject.tag == "rustyLeftHand" && Salty_Rusty_Controller.inPunchAnimationTwo)
        {
            leftHandHit = true;
            hp = hp - 50;

            if (hp == 0)
            {
                //Debug.Log("setting false...");
                Salty_Rusty_Controller.targetInRange = false;
                particleEffect.SetActive(true);
                GameObject.Destroy(transform.parent.gameObject.GetComponent<BoxCollider>());
                GameObject.Destroy(transform.parent.gameObject, 0.2f);
                
                GameObject.Destroy(transform.root.gameObject.GetComponent<Rigidbody>());

                GameObject.Destroy(rootObj, 1f);
            }

            MeleeVFXController.leftHandHit = true;
        }
    }
}
