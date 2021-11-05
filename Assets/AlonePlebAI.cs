using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AlonePlebAI : MonoBehaviour
{
    
    public enum ActionType {Attack,Wandering}
    public ActionType actionType;
    private TowDefMonster _towDefMonster;
    [NonSerialized]public bool isAttacking;

    private FovSoldier _fs;//Soldier field of view class'ı, "görüş alanı"
    private Vector3 _destination,_pathTarget;
    public Transform playerTransform;
    public NavMeshAgent agent;
    //[NonSerialized]public EnemyCharacterMovement _character;
    private bool _inCoroutine;
    private float _minDistanceToPlayer = 15f;
    
    public virtual void Start()
    {
        isAttacking = false;
        actionType = ActionType.Wandering;
        //Bunu yapmasak daha iyi olabilir tag işini
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _fs = GetComponentInChildren<FovSoldier>();
        _towDefMonster = GetComponent<TowDefMonster>();

    }
    private void Update()
    {
        switch (actionType)
        {
            case ActionType.Wandering:
                if(!_inCoroutine)
                    StartCoroutine(WanderAgain());
                break;
            case ActionType.Attack:
                var position = playerTransform.position;
                _destination = position;
                var distance = Vector3.Distance(this.transform.position, position);
                if (!isAttacking)
                {
                    agent.speed = _towDefMonster.runSpeed;
                    agent.destination = _destination;
                }
                else
                {
                    agent.speed =0;
                    agent.SetDestination(agent.transform.position);
                }
                break;
        }
    }
  
    private void PatrollingRandomly()
    {
        _pathTarget = GetNewRandomPos();
        agent.SetDestination(_pathTarget);
 
    }
    private void FaceTarget()
    {
        var direction = (playerTransform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private Vector3 GetNewRandomPos()
    {
        const int range = 40;
        var position = this.transform.position;
        var pos = new Vector3(position.x + Random.Range(-range, range), position.y,position.z + Random.Range(-range, range));
        return pos;
    }

    private IEnumerator WanderAgain()
    {
        _inCoroutine = true;
        var timeForNewPath = Random.Range(5, 10);
        yield return new WaitForSeconds(timeForNewPath);
        PatrollingRandomly();
        _inCoroutine = false;
    }
}
