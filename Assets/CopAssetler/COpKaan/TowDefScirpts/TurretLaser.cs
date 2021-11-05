using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurretLaser : Turret
{

    //public Vector3 laserOffset;
    public LineRenderer[] lasers;


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
            foreach (LineRenderer l in lasers)
            {
                l.enabled = false;
            }
            setNewTarget();

        }






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
        lasers = GetComponentsInChildren<LineRenderer>();
        monstersInRange = new List<TowDefMonster>();
        rangeCollider = GetComponent<Collider>();
        rangeCollider.enabled = true;
        startingFireCd = fireCd;



    }


    public override void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag.Equals("Monster"))
        {
            Debug.Log("SA");

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
