using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRayCast : MonoBehaviour
{
    Ray ray;
    RaycastHit hitInfo;
    Turret turret;
    
    

    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponentInParent<Turret>();    
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    public void throwRayShoot()
    {
        
        ray = new Ray(transform.position, transform.forward*1000);
        Debug.DrawRay(ray.origin,ray.direction*1000, Color.red , 0.5f);
        //Debug.DrawRay(transform.position, transform.forward * 1000, Color.black, 1f);


        if (Physics.Raycast(ray, out hitInfo, 1000))
        {
            var monster = hitInfo.collider.GetComponent<TowDefMonster>();

            if (monster != null && monster==turret.targetMonster)
            {
                TurretIce turretIce = turret as TurretIce;

                if (turretIce != null)
                {
                    turretIce.enableParticles();
                    //Damage type ice olarak çevirilebilir
                    Debug.Log("Ice damage type şimdilik physical");
                    if (hitInfo.collider.CompareTag("crit"))
                    {
                        monster.takeDamage(turret.damage, DamageType.physical,true);
                    }
                    else
                    {
                        monster.takeDamage(turret.damage, DamageType.physical, false);

                    }

                    StartCoroutine(monster.slowMonster(1f,0f));
                    turret.fireCd = turret.startingFireCd;

                    return;
                }

                TurretLaser turretLaser = turret as TurretLaser;

                if(turretLaser != null)
                {

                    foreach (LineRenderer l in turretLaser.lasers)
                    {
                        Debug.Log(l.gameObject.name);
                        l.enabled = true;
                        //l.SetPosition(0, l.transform.parent.position + l.transform.forward * laserOffset.z + l.transform.right * laserOffset.x + l.transform.up * laserOffset.y);
                        l.SetPosition(0, l.transform.parent.position);
                        l.SetPosition(1, turretLaser.targetMonster.transform.position);
                        if (hitInfo.collider.CompareTag("crit"))
                        {
                            monster.takeDamage(turret.damage, DamageType.radiation, true);
                        }
                        else
                        {
                            monster.takeDamage(turret.damage, DamageType.radiation, false);

                        }
                        turret.fireCd = turret.startingFireCd;

                        return;
                    }
                }

                TurretFlame turretFlame = turret as TurretFlame;
                if (turretFlame != null)
                {
                   
                    turretFlame.particle.enabled = true;
                    if (hitInfo.collider.CompareTag("crit"))
                    {
                        monster.takeDamage(turret.damage, DamageType.fire, true);
                    }
                    else
                    {
                        monster.takeDamage(turret.damage, DamageType.fire, false);

                    }
                    StartCoroutine(monster.flameMonster(3f, 0.2f));
                    turret.fireCd = turret.startingFireCd;

                    return;
                }

                TurretMech turretMech = turret as TurretMech;

                Debug.Log(turretMech);

                if (turretMech != null)
                {
                    
                    turretMech.enableParticles();
                    if (hitInfo.collider.CompareTag("crit"))
                    {
                        monster.takeDamage(turret.damage, DamageType.physical, true);
                    }
                    else
                    {
                        monster.takeDamage(turret.damage, DamageType.physical, false);

                    }
                    turret.fireCd = turret.startingFireCd;
                    return;
                }
            }
        } 
        
    }



}
