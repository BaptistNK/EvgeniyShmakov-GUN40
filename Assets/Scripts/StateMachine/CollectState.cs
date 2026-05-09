using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : StateMachineBehaviour
{
    private BotBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        brain = animator.GetComponent<BotBrain>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (brain.currentTarget == null) return;

        brain.agent.SetDestination(brain.currentTarget.position);

        if (brain.agent.remainingDistance < 0.6f)
        {
            // Логика "сбора"
            Debug.Log("Предмет собран!");
            Destroy(brain.currentTarget.gameObject);

            brain.currentTarget = null;
            animator.SetBool("FoundTarget", false);
            animator.SetTrigger("CollectionDone");
        }
    }
}
