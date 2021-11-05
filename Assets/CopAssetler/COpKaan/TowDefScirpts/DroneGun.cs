using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGun : MonoBehaviour
{

    public float fireRate = 0.05f;

    public GameObject laser;

    public int damage = 1;

    private float timer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fireRate)
        {
            if (Input.GetButton("Fire1")&&PlayType.type==PlayType.playType.drone)
            {
                timer = 0;
                FireGun();
            }
        }

    }

    private void FireGun()
    {
        Debug.DrawRay(transform.position, -transform.right * 100, Color.red, 2f);

        Ray ray = new Ray(transform.position, -transform.right * 100);
        RaycastHit hitInfo;
        Instantiate(laser, transform.position, Quaternion.LookRotation(ray.direction));

        if (Physics.Raycast(ray,out hitInfo , 100))
        {

            var monster = hitInfo.collider.GetComponentInParent<TowDefMonster>();
            if (monster != null)
            {
                Debug.Log("Drone damage type şimdilik radiation");
                
                if (hitInfo.collider.CompareTag("crit"))
                {
                    Debug.Log("crit");
                    monster.takeDamage(damage, DamageType.radiation,true);
                }
                else
                {
                    Debug.Log("nno crit");
                    monster.takeDamage(damage, DamageType.radiation,false);
                }

            }

        }

    }
}
