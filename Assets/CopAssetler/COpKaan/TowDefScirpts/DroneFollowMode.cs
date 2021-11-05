using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

public class DroneFollowMode : MonoBehaviour
{
    public Animator animatorDrone;
    private float yTarget;
    private bool infront = false;
    public  Vector3 RandomPlusPosition = new Vector3(0, 0, 0);
    public GameObject target;
    public float cooldown;
    private float startingCooldown;
    GameObject companion;
    public float targetDistance;
    public float okDistance;
    public float currentDistance;
    public float currentfollowSpeed;
    public float startingfollowSpeed=5;
    public bool canMove;
    private float infrontYpos=0;

    // Start is called before the first frame update
    void Start()
    {
        float infrontYpos=0;
        yTarget = 0;
        infront = false;
        SetNewTarget();
        startingCooldown = cooldown;
        canMove = true;
        companion = this.gameObject;
        startingfollowSpeed = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

            
        float angle = Vector3.Angle(target.transform.forward, transform.position-target.transform.position);
        if (Mathf.Abs(angle) < 30)
        {
            infront = true;
            // print("Object2 if front Obj1");
        }
        else
        {
            infront = false; 
        }
        currentDistance = Vector3.Distance(target.transform.position, companion.transform.position);
        
        if (!infront)
        {
            cooldown -= Time.fixedDeltaTime;
        }

        if ( cooldown<0 )
        {
            SetNewTarget();
            cooldown = startingCooldown;
        }

     
        if (infront && !FirstPersonController.IsWalkingBackward)
        {
            infrontYpos = 0;
            animatorDrone.SetBool("moving",true);

        }
        else if (infront && FirstPersonController.IsWalkingForward)
        {
            //infrontYpos=Mathf.Lerp();
            var TargetPosition = target.transform.position;
            var distance = Vector3.Distance(transform.position, TargetPosition);
            infrontYpos += Time.fixedDeltaTime;
            if (target.transform.position.y-transform.position.y>5)
            {
                var position = transform.position;
                companion.transform.position = new Vector3(position.x, position.y-infrontYpos ,position.z);
                animatorDrone.SetBool("moving",false);
            }
            
            
        }
        else
        {
            infrontYpos = 0;
            if (Vector3.Distance(transform.position,target.transform.position)>3f)
            {
                var TargetPosition = target.transform.position;
                companion.transform.position = Vector3.MoveTowards(companion.transform.position, new Vector3(TargetPosition.x+RandomPlusPosition.x,TargetPosition.y+RandomPlusPosition.y,TargetPosition.z+RandomPlusPosition.z), currentfollowSpeed * Time.deltaTime);
                animatorDrone.SetBool("moving",true);
            }
        }
      
        //companion.transform.position= new Vector3(companion.transform.position.x ,target.transform.position.y+2,companion.transform.position.z+RandomPlusPosition.z);
        if(currentDistance > targetDistance * 2)
        {
            currentfollowSpeed = startingfollowSpeed * (currentDistance / targetDistance);
        }

        
    }
    
    private void SetNewTarget()
    {
        float rastgele;
        rastgele = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        RandomPlusPosition.x = Random.Range(3, 7)*rastgele;
        RandomPlusPosition.y = Random.Range(1,3);
        rastgele = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        RandomPlusPosition.z = Random.Range(3, 7)*rastgele;
    }
    
    private void SetNewTargetBehind()
    {
        RandomPlusPosition.x = -Random.Range(3, 7);
        RandomPlusPosition.y = 0;
        RandomPlusPosition.z = -Random.Range(3, 7);
    }
    
}
