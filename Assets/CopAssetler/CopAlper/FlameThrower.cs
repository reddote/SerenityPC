using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon
{

    [System.NonSerialized]public ParticleSystem particle;
    public float fireRate;
    public RecoilScript recoil;

    public void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Stop();
    }

    [System.Obsolete]
    public override void Shoot()
    {
        //fire button = ButtonKeyController.fireButtonInputName
        if (Input.GetButtonDown(fireButton) && clipSize > 0)
        {
            particle.Play();
        }
            
        if (Input.GetButton(fireButton) && clipSize > 0)
        {
            particle.loop = true;
            recoil.Fire();

            if (Time.time - lastFireTime > fireRate)
            {
                lastFireTime = Time.time;
                clipSize -= 1;
            }
        }
        else if(particle)
        {
            particle.loop = false;
        }
        
       
    }




}
