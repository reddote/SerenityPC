using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MonsterAI))]
[RequireComponent(typeof(TowDefMonster))]

public class EnemyCharacterMovement : MonoBehaviour
{
    [SerializeField] public float m_MovingTurnSpeed = 360;
    [SerializeField] public float m_StationaryTurnSpeed = 180;
    [SerializeField] public float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] public float m_GravityMultiplier = 2f;
    [SerializeField] public float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] public float m_MoveSpeedMultiplier = 1f;
    [SerializeField] public float m_AnimSpeedMultiplier = 1f;
    [SerializeField] public float m_GroundCheckDistance = 0.5f;

    public List<AttackCollider> attackHitBoxes;
    public TowDefMonster monster;
    public MonsterAIpolish monsterAI;
    public NavMeshAgent agent;
    public Rigidbody m_Rigidbody;
    public Animator m_Animator;
    public bool m_IsGrounded;
    public float m_OrigGroundCheckDistance;
    public const float k_Half = 0.5f;
    public float m_TurnAmount;
    public float m_ForwardAmount;
    public Vector3 m_GroundNormal;
    public float m_CapsuleHeight;
    public Vector3 m_CapsuleCenter;
    public CapsuleCollider m_Capsule;
    public bool m_Crouching;
    protected float distance;

    private bool isRunning;
    private bool isWalking;
    //bool isFacingPlayer;


    public virtual void Start()
    {
        isRunning = false;
        isWalking = false;
        monster = GetComponent<TowDefMonster>();
        monsterAI = GetComponent<MonsterAIpolish>();
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        closeCollidersAnimEvent();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }


    private void Update()
    {
        UpdateAttackAnimator();
    }


    public void Move(Vector3 move)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        //Yurusun
        if (monsterAI.actionType == MonsterAIpolish.ActionType.Wandering)
        {
            m_ForwardAmount = move.z;
        }
        //Kossun
        else
        {
            m_ForwardAmount = move.z*3;
        }

        ApplyExtraTurnRotation();


        // send input and other state parameters to the animator
        UpdateAnimator(move);
        
    }
    
    

    public virtual void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        if (move.magnitude>0)
        {
            
            
            isRunning = false;
            isWalking = true;
            m_Animator.SetBool("Walk", true);
            m_Animator.SetBool("Run", false);

            if (move.magnitude>3)
            {
                isRunning = true;
                isWalking = false;
                m_Animator.SetBool("Run", true);
                m_Animator.SetBool("Walk", false);
            }
        }
        else
        {
            isRunning = false;
            isWalking = false;
            m_Animator.SetBool("Walk", false);
            m_Animator.SetBool("Run", false);
        }
        //m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        //m_Animator.SetBool("Crouch", m_Crouching);
        //m_Animator.SetBool("OnGround", m_IsGrounded);

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }

    }

    public virtual void UpdateAttackAnimator()
    {
        
    }


    public void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.5f;
    }


    public void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.5f;
        }
    }

    public void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.

        
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            // ReSharper disable once Unity.InefficientMultiplicationOrder
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }


    public void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 0.5f),Color.red);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo,m_GroundCheckDistance))
        {
            //Debug.Log("groundd");
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }

    public void closeCollidersAnimEvent()
    {
        foreach(AttackCollider attackCollider in attackHitBoxes)
        {
            attackCollider.gameObject.SetActive(false);
        }
    }
    
    
}