using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class AlonePlebMovement : MonoBehaviour
{
	[NonSerialized]public AlonePlebAI monsterAI;
	[NonSerialized]public NavMeshAgent agent;
	[NonSerialized] public Animator _animator;
	[NonSerialized] public TowDefMonster monster;
	
	
	
	[NonSerialized] public bool mirrorAttack;
	
	public List<AttackCollider> attackHitBoxes; 
	[NonSerialized]public float JumpCooldown;
	private bool isMoving;
	protected float distance;


	// Start is called before the first frame update
	public virtual void Start()
	{
		mirrorAttack = false;
		monsterAI = GetComponent<AlonePlebAI>();
		JumpCooldown = 0;
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
		if (agent.velocity.magnitude > 0.1f)
		{
			if (monsterAI.actionType.Equals(AlonePlebAI.ActionType.Attack))
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

		
		
		distance = Vector3.Distance(transform.position,monsterAI.playerTransform.position);
		//print("\ndistance="+(distance < agent.stoppingDistance * 1.01f)+"\nstamina"+(monster.stamina >= 1f )+"\nisAttacking"+(!monsterAI.isAttacking));
		if (distance < agent.stoppingDistance * 1.03f)
		{
			if (monster.stamina >= 1f && !monsterAI.isAttacking)
			{
				print("hi");
				NextBasicAttack();
			}
		}
		else
		{
//			monsterAI.isAttacking = false;
		}

	}
	
	public void Punch()
	{
		MirrorAttackAnim();
		monsterAI.isAttacking = true;
		_animator.SetBool("Punch", monsterAI.isAttacking);
		attackHitBoxes[0].gameObject.SetActive(true);
		monster.stamina -= 1;
		//StartCoroutine(EndAnimAttacking(2,"Punch"));
	}	
	
	public void UpperPunch()
	{
		MirrorAttackAnim();
		monsterAI.isAttacking = true;
		_animator.SetBool("UpperPunch", monsterAI.isAttacking);
		attackHitBoxes[0].gameObject.SetActive(true);
		monster.stamina -= 1;
		//StartCoroutine(EndAnimAttacking(2,"UpperPunch"));
	}
	
	public void closeColliders()
	{
		foreach(AttackCollider attackCollider in attackHitBoxes)
		{
			attackCollider.gameObject.SetActive(false);
		}
	}
    
	public void EndAnimAttacking(string AnimName)
	{
		// trigger the stop animation events here
		print("ending"+AnimName);
		monsterAI.isAttacking = false;
		_animator.SetBool(AnimName , monsterAI.isAttacking);
		closeColliders();
	}

	public void MirrorAttackAnim()
	{
		mirrorAttack = !mirrorAttack;
		_animator.SetBool("MirrorAttack",mirrorAttack);
	}

	public void NextBasicAttack()
	{
		switch (UnityEngine.Random.Range(1, 3))
		{
			case 1:
				Punch();
				break;
			case 2:
				UpperPunch();
				break;
		}
	}
	
}
