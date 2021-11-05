using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{
    public enum ActionType {caught,wandering,caughtByScout}

    public ActionType actionType;

    public Vector3 destination,pathTarget;
    public Transform playerTransform;
    public NavMeshAgent agent;
    public EnemyCharacterMovement character;
    private float timeForNewPath;
    bool inCoRoutine=false,shotHitted=true;
    public static bool keepWanderin=true;
    public static bool cokyakin;
    private float distance;
    float yakinlik;
    bool tooClose, dynamicWander=true;
    bool wanderlamak = true;
    int MermiSayac = 0;
    UnityEvent onTargetChange = new UnityEvent();

    

    public virtual void Start()
    {
        actionType = ActionType.wandering;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        character = GetComponent<EnemyCharacterMovement>();
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        //caughtu değiş kaan!
       onTargetChange.AddListener(caughtControl);

    }


    public virtual void Update()
    {
        yakinlik = Vector3.Distance(this.transform.position, playerTransform.position);
        //Debug.Log("a"+actionType.ToString());
        if(actionType != ActionType.caughtByScout)
        {
            if (yakinlik <= 16)
            {
                cokyakin = true;
                tooClose = true;
            }

            else if (yakinlik >= 17)
            {
                cokyakin = false;
                tooClose = false;
            }


            if (yakinlik < 110f)
            {

                caughtControl();
            }
            if (yakinlik > 110f)
            {
                wanderlamak = true;
                dynamicWander = true;
            }
            if (!inCoRoutine && dynamicWander)
            {
                StartCoroutine(wanderAgain());
                actionType = ActionType.wandering;
            }
        }
        else if (actionType == ActionType.caughtByScout)
        {
            destination = playerTransform.position;
            agent.destination = destination;
        }

        agentMovement();
    }

    public virtual void agentMovement()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity);
        else
            character.Move(Vector3.zero);

    }
    
    void FaceTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    void caughtControl()
    {

        if (FieldOfViewSoldier.caught && LightDetection.inShadow)
        {
            destination = playerTransform.position;
            distance = Vector3.Distance(this.transform.position, playerTransform.position);
            
                agent.destination = destination;
                keepWanderin = false;
                dynamicWander = false;

                //enum changing
                actionType = ActionType.caught;
                //enum changed.

            if (distance <= agent.stoppingDistance+1)
            {
                FaceTarget();
            }
            wanderlamak = false;

        }
        else if (tooClose)
        {
            destination = playerTransform.position;
            var distance = Vector3.Distance(this.transform.position, destination);
            
            
                agent.destination = destination;
                keepWanderin = false;
                dynamicWander = false;

                //enum changing
                actionType = ActionType.caught;
                //enum changed.
            
            if (distance <= agent.stoppingDistance+1)
            {
                FaceTarget();
                
            }
        }
        if (wanderlamak &&!tooClose)
        {

            keepWanderin = true;
            FieldOfViewSoldier.caught = false;
            
        }
        
       

    }
    void patrollingRandomly()
    {
        pathTarget = getNewRandomPos();
        agent.SetDestination(pathTarget);
 
    }

    Vector3 getNewRandomPos()
    {
       // float x = ;
       // float z = ;

        Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-40, 40), this.transform.position.y,this.transform.position.z + Random.Range(-40, 40));
        return pos;
    }

    IEnumerator wanderAgain()
    {
        inCoRoutine = true;
        timeForNewPath = Random.Range(9, 14);
        yield return new WaitForSeconds(timeForNewPath);
        patrollingRandomly();
        inCoRoutine = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        /*if (col.tag == "EgunBullet")
        {
            MermiSayac++;
            if (MermiSayac == 3)
                Destroy(this.gameObject);
        }*/
        

    }

}
