using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanMovement : EnemyCharacterMovement
{

    //for Soldier hitbox standart attack is 0 ,...
    public bool isAttacking=false;




    public override void UpdateAnimator(Vector3 move)
    {
        base.UpdateAnimator(move);

        if (monsterAI.playerTransform)
        {

            if (Vector3.Distance(monsterAI.playerTransform.position, transform.position) < agent.stoppingDistance * 1.3f && monster.stamina >= 1f)
            {
               standartAttack();
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
        m_Animator.SetBool("isStandardAttacking", isAttacking);
        m_Animator.SetLayerWeight(1, 1);
        attackHitBoxes[0].gameObject.SetActive(true);
        monster.stamina -= 1;
        //monster.standardAttack();
    }


}
