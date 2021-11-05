using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaMovement : EnemyMovement
{
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //print(!monsterAI.isAttacking);
        distance = Vector3.Distance(monsterAI.playerTransform.position, transform.position);
        //print(JumpCooldown);

        if (distance < agent.stoppingDistance * 1.01f && monster.stamina >= 1f && !monsterAI.isAttacking)
        {
            standartAttack();
        }
//        else if (distance>5&&distance<15 && JumpCooldown<0)
//        {
//            jumpAttack(); 
//        }
//        
    }
    
    public void standartAttack()
    {
        monsterAI.isAttacking = true;
        _animator.SetBool("StandardAttack", monsterAI.isAttacking);
        attackHitBoxes[0].gameObject.SetActive(true);
        monster.stamina -= 1;
        StartCoroutine(EndAnimAttacking(2));
    }
    public void jumpAttack()
    {
        monsterAI.isAttacking = true;
        JumpCooldown = 10;
        var target = monsterAI.playerTransform.position;
        monsterAI.agent.destination = target;
        _animator.SetBool("JumpAttack", true);
        StartCoroutine(EndAnimJumpAttacking(3,transform.position));
    }
    
    private IEnumerator StayAir()
    {
        yield return new WaitForEndOfFrame();
        var targetDistance = Vector3.Distance(transform.position, agent.destination);
        if (targetDistance > 10)
        {
            _animator.speed = 1;
        }
        else
        {
            _animator.speed = Mathf.Lerp(0,1,1/(targetDistance));
        }
    }
    
    
    IEnumerator EndAnimAttacking(float AnimLenght){
        yield return new WaitForSeconds(AnimLenght);
        // trigger the stop animation events here
        monsterAI.isAttacking = false;
        _animator.SetBool("StandardAttack", monsterAI.isAttacking);
        closeColliders();
    }  
    
    IEnumerator EndAnimJumpAttacking(float AnimLenght , Vector3 initialPos)
    {
        float time= 0;
        time += Time.deltaTime;
        print(time);
        transform.position =Vector3.Slerp(initialPos,monsterAI.playerTransform.position, time/AnimLenght);
        
        yield return new WaitForSeconds(AnimLenght);
        // trigger the stop animation events here
        monsterAI.isAttacking = false;
        _animator.SetBool("JumpAttack", monsterAI.isAttacking);
        closeColliders();
        print("saa");
    }
    
    
}
