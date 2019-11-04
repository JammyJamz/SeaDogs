using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BaseProjectile
{
    public float hangTime;

    bool fired;

    Vector3 direction;
 
    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            transform.position += direction * (speed * Time.deltaTime);
        }

        Tidy();
    }

    public override void FireProjectile(GameObject projectile, GameObject target)
    {
        if (projectile && target)
        {
            direction = (target.transform.position - projectile.transform.position).normalized;
            fired = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destroy Projectile");
        Destroy(this.gameObject);
    }

    void Tidy()
    {
        Destroy(this.gameObject, hangTime);
    }
}
