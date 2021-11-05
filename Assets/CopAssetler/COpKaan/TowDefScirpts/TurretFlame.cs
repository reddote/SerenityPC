using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurretFlame : Turret
{

    //public Vector3 laserOffset;
    //public LineRenderer[] lasers;
    public ParticleSystemRenderer particle;


    // Start is called before the first frame update
    void Start()
    {
        setComponents();
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        if (targetMonster == null)
        {
            particle.enabled = false;
            setNewTarget();

        }
        //else
        //{
        //    particle.enabled = true;
        //}






    }

    public override void fireTurret()
    {

        fireCd -= Time.deltaTime;


        if (fireCd < 0 && targetMonster != null)
        {


            rayCast.throwRayShoot();

        }

    }


    public override void nullTarget()
    {
        if (targetMonster != null && !targetMonster.isAlive)
        {
            targetMonster = null;
        }
    }





    public override void setComponents()
    {
        particle = GetComponentInChildren<ParticleSystemRenderer>();
        particle.enabled = true;
        monstersInRange = new List<TowDefMonster>();
        rangeCollider = GetComponent<Collider>();
        rangeCollider.enabled = true;
        startingFireCd = fireCd;
    }


    public override void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag.Equals("Monster"))
        {
           
            if (!monstersInRange.Contains(other.gameObject.GetComponent<TowDefMonster>()))
            {
                monstersInRange.Add(other.gameObject.GetComponent<TowDefMonster>());
            }
        }



    }


    public override void OnTriggerExit(Collider other)
    {

        TowDefMonster reference = other.gameObject.GetComponent<TowDefMonster>();

        if (reference && monstersInRange.Contains(reference))
        {
            monstersInRange.Remove(reference);
            if (reference.Equals(targetMonster))
            {
                targetMonster = null;
            }
        }

    }





}
