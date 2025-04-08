using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonVikingwAction : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject waveGameObject = Instantiate(animator.transform.parent.transform.GetComponent<CartoonViking>().CartoonVikingWave, animator.transform.parent.transform.position, animator.transform.parent.rotation);
        waveGameObject.transform.localScale = new Vector3(-animator.transform.parent.transform.localScale.x, 1, 1);
        waveGameObject.GetComponent<AttackSource>().attackSource = animator.transform.parent.transform;
        animator.transform.parent.transform.GetComponent<CartoonViking>().action = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
