using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [NonSerialized]public MonsterAIpolish monsterAI;
    [NonSerialized]public NavMeshAgent agent;
    [NonSerialized] public Animator _animator;
    [NonSerialized] public TowDefMonster monster;
    public List<AttackCollider> attackHitBoxes; 
    [NonSerialized]public float JumpCooldown;
    private bool isMoving;
    private bool jumpAttacking;
    protected float distance;


    // Start is called before the first frame update
    public virtual void Start()
    {
        monsterAI = GetComponent<MonsterAIpolish>();
        JumpCooldown = 0;
        monsterAI.isAttacking = false;
        isMoving = false;
        monster = GetComponent<TowDefMonster>();
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        closeColliders();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //print(monsterAI.isAttacking);
        JumpCooldown -= Time.deltaTime;
        if (agent.velocity.magnitude > 0.1f)
        {
            if (agent.velocity.magnitude > monster.runSpeed-0.1f)
            {
                _animator.SetBool("Run",true);
                _animator.SetBool("Walk",false);
                isMoving = true;
            }
            else
            {
                _animator.SetBool("Walk",true);
                _animator.SetBool("Run",false);
                isMoving = true;
            }
        }
        else
        {
            _animator.SetBool("Walk",false);
            _animator.SetBool("Run",false);
            isMoving = false;
        }
      
        
    }
    
    
    public void closeColliders()
    {
        foreach(AttackCollider attackCollider in attackHitBoxes)
        {
            attackCollider.gameObject.SetActive(false);
        }
    }
 
}
