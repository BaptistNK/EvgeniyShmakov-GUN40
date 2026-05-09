using UnityEngine;
using UnityEngine.AI;

public class ReturnState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private CharacterBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        brain = animator.GetComponent<CharacterBrain>();

        // Отправляем персонажа к базе
        if (brain.basePoint != null)
        {
            agent.SetDestination(brain.basePoint.position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Проверяем, дошел ли персонаж до базы
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            brain.UnloadItems(); // Сбрасываем груз
            animator.SetBool("IsFull", false); // Возвращаемся в Idle
        }
    }
}