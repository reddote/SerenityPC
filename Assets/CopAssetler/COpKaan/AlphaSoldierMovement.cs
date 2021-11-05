using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSoldierMovement : EnemyCharacterMovement
{
    
   
    //for Soldier hitbox standart attack is 0 ,...
    public bool isAttacking = false;
    public bool jumpAttacking;

    public override void Start()
    {
        base.Start();
        jumpAttacking = false;
    }


//    public override void UpdateAnimator(Vector3 move)
//    {
//        base.UpdateAnimator(move);
//    }

    public override void UpdateAttackAnimator()
    {
        base.UpdateAttackAnimator();
        if (monsterAI.actionType == MonsterAIpolish.ActionType.Caught)
        {
            distance = Vector3.Distance(monsterAI.playerTransform.position, transform.position);
            //Debug.Log(distance);    
            if (distance < agent.stoppingDistance * 1.1f && monster.stamina >= 1f && !jumpAttacking)
            {
               
                standartAttack();
            }
            else if (distance>10&&distance<15)
            {
                jumpAttack(); 
            }

        }
        else if (!isAttacking)
        {
            m_Animator.SetLayerWeight(1, 0);
        }

    }

    public void standartAttack()
    {
        isAttacking = true;
        m_Animator.SetBool("StandardAttack", isAttacking);
        m_Animator.SetLayerWeight(1, 1);
        attackHitBoxes[0].gameObject.SetActive(true);
        monster.stamina -= 1;
        //monster.standardAttack();
    }
 
    public void jumpAttack()
    {
        jumpAttacking = true;
        var target = monsterAI.playerTransform.position;
        monsterAI.agent.destination = target;
        m_Animator.SetBool("JumpAttack", true);
        StartCoroutine(StayAir());
        //attackHitBoxes[0].gameObject.SetActive(true);
        //monster.stamina -= 1;
        //monster.standardAttack();
    }

    
    
    public void jumpAttackColliderOpenAnimEvent()
    {
        //const int yapabilirsin
        attackHitBoxes[1].gameObject.SetActive(true);
    }
    
    public void jumpAttackColliderCloseAnimEvent()
    {
        attackHitBoxes[1].gameObject.SetActive(false);
    }
    
    public void endStandartAttackAnimEvent()
    { 
        //Debug.Log(Vector3.Distance(monsterAI.playerTransform.position, transform.position));
        if (distance>agent.stoppingDistance * 1.1f)
        {
            isAttacking = false;
            m_Animator.SetBool("StandardAttack", isAttacking);
        }
    
    } 
    
    //Animation eventte çağırılıyor
    public void endJumpAttackAnimEvent()
    {
        jumpAttacking = false;
        m_Animator.SetBool("JumpAttack", false);
        m_Animator.speed = 1;

    }
    
    private IEnumerator StayAir()
    {
        yield return new WaitForEndOfFrame();
        var targetDistance = Vector3.Distance(transform.position, agent.destination);
        if (targetDistance > 10)
        {
            m_Animator.speed = 1;
        }
        else
        {
            m_Animator.speed = Mathf.Lerp(0,1,1/(targetDistance));
        }
    }

}
