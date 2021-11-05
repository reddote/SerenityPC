using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TurretIce : Turret
{

    //public Vector3 laserOffset;
    //public LineRenderer[] lasers;
    public ParticleSystemRenderer[] particles;


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
           
            disableParticles();
            setNewTarget();

        }






    }

    private void disableParticles()
    {
        foreach (ParticleSystemRenderer particle in particles) {
            particle.gameObject.SetActive(false);
        }
    }
    
    public void enableParticles()
    {
        foreach (ParticleSystemRenderer particle in particles) {
            particle.gameObject.SetActive(true);
        }
    }

    public override void fireTurret()
    {

        fireCd -= Time.deltaTime;


        if (fireCd < 0 && targetMonster != null)
        {
            //Kod şu an turret ateş ederse cdyi sıfırlıyor ve her bir class için diğer methodları özel olarak yapıyor.
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
        particles = GetComponentsInChildren<ParticleSystemRenderer>();
        disableParticles();
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
