using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.velocity = Vector3.zero;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Собрал");
    }
}
