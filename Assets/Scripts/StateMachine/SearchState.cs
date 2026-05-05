using System;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : StateMachineBehaviour
{
    NavMeshAgent agent;
    float searchRadius = 10f;
    float detectionRadius = 5f;
    LayerMask itemLayer = LayerMask.GetMask("Items");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomDestination(animator.transform.position);
        }
        Collider[] items = Physics.OverlapSphere(animator.transform.position, detectionRadius, itemLayer);
        if(items.Length > 0)
        {
            animator.SetTrigger("foundItem");
        }
    }

    private void SetRandomDestination(Vector3 currentPosition)
    {
        Vector3 randomDir = UnityEngine.Random.insideUnitSphere * searchRadius;
        randomDir += currentPosition;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomDir,out hit, searchRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}
