using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrumblingMonster : TowDefMonster
{
    public NavMeshHit navMeshHit;
    public GameObject crumbleMonster;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Health = 300;
        MaxHealth = Health;
        monsterSize = Size.colossal;
        moveSpeed = 5;
        baseSpeed = moveSpeed;
        monsterMeshAgent.speed = moveSpeed;
        Debug.Log(baseSpeed);
        Damage = SizeRng(monsterSize) + (5 * Rank); ;

    }


    public override void DieMonster()
    {
        Debug.Log("DİEEE");

        isAlive = false;
        SpawnPoint.livingTowDefMonsters.Remove(this.gameObject);
        SpawnPoint.livingTowDefMonsters.Add(Instantiate(crumbleMonster, NavMeshHitUtility.RandomPointOnNavMesh(navMeshHit, transform.position, 10f), Quaternion.identity));
        SpawnPoint.livingTowDefMonsters.Add(Instantiate(crumbleMonster, NavMeshHitUtility.RandomPointOnNavMesh(navMeshHit, transform.position, 10f), Quaternion.identity));
        SpawnPoint.livingTowDefMonsters.Add(Instantiate(crumbleMonster, NavMeshHitUtility.RandomPointOnNavMesh(navMeshHit, transform.position, 10f), Quaternion.identity));    
        Destroy(this.gameObject);

    }




}
