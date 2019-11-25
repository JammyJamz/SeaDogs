using UnityEngine;
using System.Collections;

public class TrackingProjectile : BaseProjectile
{
  GameObject newtarget;
  public float hangTime = 10f;
  void Update()
  {
        if (newtarget)
            transform.position = Vector3.MoveTowards(transform.position, newtarget.transform.position, speed * Time.deltaTime);

        StartCoroutine(Tidy());
    }

  public override void FireProjectile(GameObject projectile, GameObject target)
  {
    if (target)
      newtarget = target;
  }

    IEnumerator Tidy()
    {
        yield return new WaitForSeconds(hangTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destroy Projectile");
        Destroy(this.gameObject);
    }

}
