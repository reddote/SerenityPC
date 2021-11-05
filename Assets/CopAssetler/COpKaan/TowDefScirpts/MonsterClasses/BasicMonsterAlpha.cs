using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterAlpha : TowDefMonster
{


    public override void Start()
    {
        base.Start();
        Rank = 1;
        monsterSize = Size.medium;
        moveSpeed = 4;
        runSpeed = moveSpeed * 2;
        baseSpeed = moveSpeed;
        print(runSpeed);


        if (Rank == 1)
        {
            Health = 100;
            maxStamina = 3.0f;
            stamina = maxStamina;
            staminaRecovery = 0.1f;
            //resTypes.Add(new Resistance(DamageType.physical, 4));
            //resTypes.Add(new Resistance(DamageType.fire, 4));

            MaxHealth = Health;
            monsterMeshAgent.speed = moveSpeed;
            Damage = SizeRng(monsterSize) + (5 * Rank);
        }

    }



}
