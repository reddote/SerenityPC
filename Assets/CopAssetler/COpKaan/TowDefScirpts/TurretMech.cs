using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TurretMech : Turret
{

    //public Vector3 laserOffset;
    //public LineRenderer[] lasers;
    public ParticleSystem[] particles;


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
        foreach (ParticleSystem particle in particles)
        {
          
            particle.Clear();
        }
    }

    public void enableParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
    

    public override void setComponents()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            var main = particle.main;
            main.loop = false;
        }
        disableParticles();
        monstersInRange = new List<TowDefMonster>();
        rangeCollider = GetComponent<Collider>();
       
        startingFireCd = fireCd;
    }


}
