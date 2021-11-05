using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{

    //public Vector3 laserOffset;
    //public LineRenderer[] lasers;

    public bool turretModeActive;
    public float fireCd;
    public float startingFireCd;
    public float damage;
    public TowDefMonster targetMonster;
    public List<TowDefMonster> monstersInRange;
    public Collider rangeCollider,colliderr;
    public int price;
    public TurretRayCast rayCast;
    public Material hologramMaterial;
    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    public List<Material> startingMaterials = new List<Material>();
  


    public List<TowDefMonster> AliveMonsterInRange
    {
        get
        {
            return monstersInRange
                .Where(monster => monster.isAlive)
                .ToList();
        }
    }

    public virtual void Update()
    {
        if (turretModeActive)
        {
            Debug.Log("hi");
            nullTarget();
            fireTurret();
        }
    }


    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            startingMaterials.Add(meshRenderers[i].material);
        }
        turretModeActive = false;
        rangeCollider = GetComponents<Collider>().ToList().FirstOrDefault(col => col.isTrigger);
        if (rangeCollider != null) rangeCollider.enabled = true;
        colliderr = GetComponents<Collider>().ToList().FirstOrDefault(col => !col.isTrigger);
 
        rayCast = GetComponentInChildren<TurretRayCast>();
    }


    public virtual void fireTurret()
    {

        fireCd -= Time.deltaTime;


        if (fireCd < 0 && targetMonster != null && turretModeActive)
        {
            //Kod şu an turret ateş ederse cdyi sıfırlıyor.
            rayCast.throwRayShoot();

        }

    }


    public virtual void nullTarget()
    {
        if (targetMonster != null && !targetMonster.isAlive)
        {
            targetMonster = null;
        }
    }

    public virtual void setNewTarget()
    {
        float minDistance = 10000;


        foreach (TowDefMonster monsterInRange in AliveMonsterInRange)
        {

            if (monsterInRange == null)
            {
                continue;
            }

            
            float currentDistance = Vector3.Distance(monsterInRange.transform.position, transform.position);


            if (currentDistance < minDistance)
            {
                targetMonster = monsterInRange;
                minDistance = currentDistance;
            }
        }

    }




    public virtual void setComponents()
    {
        
        monstersInRange = new List<TowDefMonster>();
       
        startingFireCd = fireCd;



    }


    public virtual void OnTriggerEnter(Collider other)
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


    public virtual void OnTriggerExit(Collider other)
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
