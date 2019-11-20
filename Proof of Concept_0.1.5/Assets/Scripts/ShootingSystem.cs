using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public float fireRate;
    public float fieldOfView;
    public bool beam;

    public GameObject projectile;
    private Transform target;
    //private Transform targetTransform;

    public Transform saltyPos;
    public Transform rustyPos;

    public List<GameObject> projectileSpawns;

    float fireTimer = 0f;


    List<GameObject> lastProjectiles = new List<GameObject>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        // target = GameObject.FindGameObjectsWithTag("Player")[0];
        if (Salty_Rusty_Controller.isSalty)
            target = saltyPos;

        else
            target = rustyPos;

        Debug.Log(target.name);
       // targetTransform = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (beam && lastProjectiles.Count <= 0)
        {
            //float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(transform.position - target.transform.position));
            // ^ Value never used
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance < fieldOfView)
                SpawnProjectiles();
        }

        else if (beam && lastProjectiles.Count > 0)
        {
            //float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position));
            // ^ Value never used
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance > fieldOfView)
            {
                while (lastProjectiles.Count > 0)
                {
                    Destroy(lastProjectiles[0]);
                    lastProjectiles.RemoveAt(0);
                }
            }

        }

        else
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                //float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position));
                // ^ Value never used
                float distance = Vector3.Distance(target.position, transform.position);

                if (distance < fieldOfView)
                {
                    SpawnProjectiles();
                    fireTimer = 0f;
                }
            }
        }
        

        

    }

    void SpawnProjectiles()
    {
        int i;

        if (!projectile)
        {
            return;
        }

        lastProjectiles.Clear();

        for (i = 0; i < projectileSpawns.Count; i++)
        {
            if (projectileSpawns[i])
            {
                GameObject proj = Instantiate(projectile, projectileSpawns[i].transform.position, Quaternion.Euler(projectileSpawns[i].transform.forward));
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], target);

                lastProjectiles.Add(proj);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfView);
    }
}
