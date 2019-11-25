using UnityEngine;
using System.Collections;

public class TrackingProjectile : BaseProjectile
{
  GameObject newtarget;

  void Update()
  {
    if (newtarget)
      transform.position = Vector3.MoveTowards(transform.position, newtarget.transform.position, speed * Time.deltaTime)
  }

  public override void FireProjectile(GameObject projectile, GameObject target)
  {
    if (target)
      newtarget = target;
  }

}
