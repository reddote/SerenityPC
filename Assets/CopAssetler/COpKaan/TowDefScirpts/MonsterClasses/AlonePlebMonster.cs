using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlonePlebMonster :TowDefMonster
{

    private AlonePlebAI plebAI;
    private AlonePlebMovement plebMovement;
    
    //Editorde Save olur ancak akının bunları kaydetmesi gerekiyor build için.
    public static Dictionary<DamageType,bool> resistanceDisctionary = new Dictionary<DamageType, bool>()
    {
        //{DamageType.fire,false},
        {DamageType.physical,false}
    };
    public static Dictionary<DamageType, bool> weaknessDictionary = new Dictionary<DamageType, bool>()
    {
        {DamageType.fire,false}
       // { DamageType.physical,false}
    };
    public static Dictionary<DamageType, bool> immunityDiscovered = new Dictionary<DamageType, bool>()
    {

    };
    
   

    public override void Start()
    {
        base.Start();
        plebAI = GetComponent<AlonePlebAI>();
        plebMovement = GetComponent<AlonePlebMovement>();
        Rank = 1;
        monsterSize = Size.medium;
        moveSpeed = 2f;
        baseSpeed = moveSpeed;
        runSpeed = 5f;
        baseRunSpeed = runSpeed;
        monsterMeshAgent.speed = moveSpeed;
   
      

        if (Rank == 1)
        {
            Health = 50;
            maxStamina = 2.0f;
            stamina = maxStamina;
            staminaRecovery = 0.1f;
            resTypes.Add(new Resistance(DamageType.physical, 3, resistanceDisctionary[DamageType.physical]));
            //resTypes.Add(new Resistance(DamageType.fire, 3 , resistanceDisctionary[DamageType.fire    ]));
            weakTypes.Add(new Weakness(DamageType.fire,weaknessDictionary[DamageType.fire]));
            
            MaxHealth = Health;   
            monsterMeshAgent.speed = moveSpeed;
            Damage = SizeRng(monsterSize) + (5 * Rank);
        }

    }


    public override void takeDamage(float hitDamage, DamageType damageType, bool isCrit=false)
    {
        for (int i = 0; i < resTypes.Count; i++)
        {

            if (resTypes[i].resistanceType.Equals(damageType))
            {
                
                hitDamage -= resTypes[i].damageRedcution;
                if(!resistanceDisctionary[damageType])
                {

                    DiscoveryString = (damageType + " resistance discovered");
                    resistanceDisctionary[damageType] = true;
                    player.Discover(DiscoveryString);
                    //Önemli olan basicmonsterdaki discovered diğeri sadece ilk başta kullanılıyor
                    //ve sonrasında spawnlananlar buna göre spawnlanıyor buna göre spawnlanmasa bile 
                    //sorun olmazdı da neyse xdxdxdxdxdxDX;D X;DX;DX;DX;
                    //resTypes[i].isDiscovered = true;
                }
                break;
            }
        }
        for (int i = 0; i < weakTypes.Count; i++)
        {

            if (weakTypes[i].weaknessType.Equals(damageType))
            {
                hitDamage *= 2;
                Debug.Log(hitDamage);
                if (!weaknessDictionary[damageType])
                {

                    DiscoveryString = (damageType + " weakness discovered");
                    weaknessDictionary[damageType] = true;
                    player.Discover(DiscoveryString);
                    //Önemli olan basicmonsterdaki discovered diğeri sadece ilk başta kullanılıyor
                    //ve sonrasında spawnlananlar buna göre spawnlanıyor buna göre spawnlanmasa bile 
                    //sorun olmazdı da neyse xdxdxdxdxdxDX;D X;DX;DX;DX;
                    //resTypes[i].isDiscovered = true;
                }
                break;
            }
        }
        for (int i = 0; i < immuneTypes.Count; i++)
        {

            if (immuneTypes[i].immunityType.Equals(damageType))
            {
                hitDamage = 0;
                Debug.Log(hitDamage);
                if (!immunityDiscovered[damageType])
                {

                    DiscoveryString = (damageType + " immunity discovered");
                    immunityDiscovered[damageType] = true;
                    player.Discover(DiscoveryString);
                    //Önemli olan basicmonsterdaki discovered diğeri sadece ilk başta kullanılıyor
                    //ve sonrasında spawnlananlar buna göre spawnlanıyor buna göre spawnlanmasa bile 
                    //sorun olmazdı da neyse xdxdxdxdxdxDX;D X;DX;DX;DX;
                    //resTypes[i].isDiscovered = true;
                }
                break;
            }
        }
        if (isCrit)
        {
            hitDamage = hitDamage * 2;
            Debug.Log("Critical Hit DAMAGE=" + hitDamage);
        }
        if (hitDamage < 0)
        {
            hitDamage = 0;
        }
        Health -= hitDamage;
        plebAI.actionType = AlonePlebAI.ActionType.Attack;
        plebMovement._animator.SetBool("Attack",true);
        //Debug.Log(Health);
        if (Health < 1 && isAlive)
        {
            //DieMonster();
            Destroy(this.gameObject);
        }

    }




}
