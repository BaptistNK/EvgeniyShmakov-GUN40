using System;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : StateMachineBehaviour
{
    private BotBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        brain = animator.GetComponent<BotBrain>();

        // Проверяем, назначен ли агент, если нет — пробуем найти его
        if (brain != null && brain.agent == null)
            brain.agent = animator.GetComponent<NavMeshAgent>();

        if (brain != null && brain.agent != null)
            MoveToRandomPos(brain);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (brain == null || brain.agent == null) return;

        // Если бот дошел до точки — выбираем новую
        if (!brain.agent.pathPending && brain.agent.remainingDistance < 0.5f)
        {
            MoveToRandomPos(brain);
        }

        // Поиск целей
        Collider[] targets = Physics.OverlapSphere(brain.transform.position, brain.viewRadius, brain.targetMask);
        if (targets.Length > 0)
        {
            // ИСПРАВЛЕНО: берем ТРАНСФОРМ первого элемента массива [0]
            brain.currentTarget = targets[0].transform;
            animator.SetBool("FoundTarget", true);
        }
    }

    void MoveToRandomPos(BotBrain brain)
    {
        if (brain.agent == null) return;

        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 10f;
        randomPos += brain.transform.position;

        NavMeshHit hit;
        // Ищем ближайшую точку на NavMesh в радиусе 10 метров
        if (NavMesh.SamplePosition(randomPos, out hit, 10f, NavMesh.AllAreas))
        {
            brain.agent.SetDestination(hit.position);
        }
    }
}
