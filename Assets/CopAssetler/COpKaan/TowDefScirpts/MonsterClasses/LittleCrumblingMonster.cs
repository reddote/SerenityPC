using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleCrumblingMonster : CrumblingMonster
{
    


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
        Health = 100;
        MaxHealth = Health;
        moveSpeed = 10;
        baseSpeed = moveSpeed;
        monsterSize = Size.small;
        monsterMeshAgent.speed = moveSpeed;
        Damage = SizeRng(monsterSize) + (5 * Rank);
      
    }


    public override void DieMonster()
    {

        isAlive = false;
        SpawnPoint.livingTowDefMonsters.Remove(this.gameObject);
        SpawnPoint.livingTowDefMonsters.Add(Instantiate(crumbleMonster, NavMeshHitUtility.RandomPointOnNavMesh(navMeshHit, transform.position, 10f), Quaternion.identity));
        SpawnPoint.livingTowDefMonsters.Add(Instantiate(crumbleMonster, NavMeshHitUtility.RandomPointOnNavMesh(navMeshHit, transform.position, 10f), Quaternion.identity));
        Destroy(this.gameObject);

    }

}
