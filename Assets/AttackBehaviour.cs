using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{

    public string stateName;
    AlonePlebMovement pleb;
    
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        pleb = animator.GetComponent<AlonePlebMovement>();
        pleb.EndAnimAttacking(stateName);
	}

}