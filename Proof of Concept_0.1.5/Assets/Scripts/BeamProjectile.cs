using UnityEngine;
using System.Collections;

public class BeamProjectile : BaseProjectile
{
  GameObject newprojectile;
  public float beamLength = 10f;

  void Update()
  {
    if (newprojectile)
      GetComponent<LineRenderer>().SetPosition(0, newprojectile.transform.position);
      GetComponent<LineRenderer>().SetPosition(1, newprojectile.transform.position + (newprojectile.transform.forward * beamLength));
  }

  public override void FireProjectile(GameObject projectile, GameObject target)
  {
    if (projectile)
    {
      newprojectile = projectile;
      transform.SetParent(newprojectile.transform);
    }

  }

}
