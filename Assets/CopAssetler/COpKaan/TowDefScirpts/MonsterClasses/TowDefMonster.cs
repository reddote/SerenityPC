using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Size { tiny,small,medium,large,huge,gargantuan,colossal};


public class TowDefMonster : MonoBehaviour
{

    //public List<AttackCollider> attackHitBoxes;
    public Renderer rend;
    public NavMeshAgent monsterMeshAgent;
    public EnemyCharacterMovement enemyCharacterMovement;
    public MonsterAIpolish monsterAI;
    //diğerlerini de Resistanceye benzetebilirsin...
    public List<Resistance> resTypes;
    public List<Weakness> weakTypes;
    public List<Immunity> immuneTypes;
    public Size monsterSize;
    //weakness ve immune yap
   

    public int Rank;
    public float maxStamina;
    public float stamina;
    public float staminaRecovery;
    public float Health;
    public float MaxHealth;
    public float moveSpeed;
    public float baseSpeed;
    public float runSpeed;
    public float baseRunSpeed;
    public float Damage;
    public bool isAlive=true;
    public bool inInstantCooldown=false;
    public string DiscoveryString;
    

    public Player player;

    public bool slowEffectCoroutine,flameEffectCoroutine;
    float BurnDuration=3f;
    public static int deathCount = 0;

    public virtual void Start()
    {
        
        monsterAI = GetComponent<MonsterAIpolish>();
        monsterMeshAgent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        resTypes = new List<Resistance>();
        weakTypes = new List<Weakness>();
        immuneTypes = new List<Immunity>();
        player = FindObjectOfType<Player>();
        //Debug.Log(resTypes[0]);
        InvokeRepeating("recover",0,0.1f);
       // StartCoroutine(RecoverStamina());
    }

    

    public void recover()
    {
    
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        if (stamina < maxStamina)
        {
            //Debug.Log("stamina" + (float)stamina);
            stamina += staminaRecovery;
        }
        //Debug.Log("Working, " + stamina);
    }


    IEnumerator RecoverStamina()
    {
        while (true) // this just equates to "repeat forever"
        {
            if (stamina < maxStamina)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("After" + stamina);
                stamina += staminaRecovery;
                yield return null;

            }
            else
            {
                yield return null;
            }
        }
    }






    //Damage Type Al parametre olarak ona göre resistance vs eklersin.
    public virtual void takeDamage(float hitDamage,DamageType damageType, bool isCrit=false)
    {
        for (int i = 0; i <resTypes.Count ; i++)
        {
            
            if (resTypes[i].resistanceType.Equals(damageType))
            {
                Debug.Log("Res type=" + resTypes[i].resistanceType + "Damage=" + damageType);
                hitDamage -= resTypes[i].damageRedcution;
                Debug.Log(hitDamage);
                break;
            }
        }
        for (int i = 0; i < weakTypes.Count; i++)
        {

            if (weakTypes[i].weaknessType.Equals(damageType))
            {
                hitDamage *= 2;
                Debug.Log(hitDamage);
                break;
            }
        }
        for (int i = 0; i < immuneTypes.Count; i++)
        {

            if (immuneTypes[i].immunityType.Equals(damageType))
            {
                hitDamage = 0;
                Debug.Log(hitDamage);
                break;
            }
        }
        if (isCrit)
        {
            hitDamage = hitDamage * 2;
        }
        if (hitDamage < 0)
        {
            hitDamage = 0;
        }
        Health -= hitDamage;
        //Debug.Log(Health);
        if (Health < 1 && isAlive)
        {
            //DieMonster();
            Destroy(this.gameObject);
        }

    }

    public virtual void DieMonster()
    {
        isAlive = false;
        SpawnPoint.livingTowDefMonsters.Remove(this.gameObject);
        deathCount++;
        StartCoroutine(FadeNdestroy());
    }

    public IEnumerator FadeNdestroy()
    {
        //rend = GetComponent<Renderer>();

        //for (float f = 1f; f >= 0; f -= Time.deltaTime)
        //{
        //    Color c = rend.material.color;
        //    c.a = f;
        //    rend.material.color = c;

        //    yield return null;
        //}

        Destroy(this.gameObject);
        yield return null;
    }

     public IEnumerator slowMonster(float duration, float slowPercentage)
     {
        
        slowEffectCoroutine = true;
        if(monsterMeshAgent!=null)
        monsterMeshAgent.speed = baseSpeed * slowPercentage/100;
        yield return new WaitForSeconds(duration);
        if (monsterMeshAgent != null)
        monsterMeshAgent.speed = moveSpeed;
        slowEffectCoroutine = false;
     }
       public IEnumerator flameMonster(float maxDur, float damage)
       {
        if (flameEffectCoroutine)
        {
            BurnDuration = maxDur;
            yield break;
        }

        flameEffectCoroutine = true;

        float counter = 0f;
        float damageInterval = 0.5f; 

        while(BurnDuration > 0f && monsterMeshAgent != null)
        {
            Debug.Log("Burnin");
            counter += Time.deltaTime; 

            if(counter > damageInterval)
            {
                
                counter = 0f;
                BurnDuration -= damageInterval;
                takeDamage(damage,DamageType.fire);
            }

            yield return null;
        }

        flameEffectCoroutine = false;
        BurnDuration = maxDur;      
     }



    private void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("FlameParticle"))
        {

            StartCoroutine(flameMonster(3f, 0.2f));
        }

    }


    public int SizeRng(Size size)
    {
        
        
        if (monsterSize==Size.tiny)
        {
            return UnityEngine.Random.Range(1, 3);
        }
        else if (monsterSize == Size.small)
        {
            return UnityEngine.Random.Range(3, 5);
        }
        else if (monsterSize == Size.medium)
        {
            return UnityEngine.Random.Range(4, 7);
        }
        else if (monsterSize == Size.large)
        {
            return UnityEngine.Random.Range(5, 9);
        }
        else if (monsterSize == Size.huge)
        {
            return UnityEngine.Random.Range(6, 11);
        }
        else if (monsterSize == Size.gargantuan)
        {
            return UnityEngine.Random.Range(7, 13);
        }
        else if (monsterSize == Size.colossal)
        {
            return UnityEngine.Random.Range(11, 21);
        }


        return 0;
    }


}
