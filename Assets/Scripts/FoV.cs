using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FoV : MonoBehaviour
{
    [Header("Настройки обзора")]
    public float viewRadius = 10f;
    [Range(0f, 360f)]
    public float viewAngle = 90f;
    public LayerMask targetMask;   // Слой объектов (например, "Items")
    public LayerMask obstacleMask; // Слой стен

    [Header("Сбор объектов")]
    public List<Transform> visibleTargets = new List<Transform>();
    private NavMeshAgent agent;
    private Transform currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Запускаем поиск целей 5 раз в секунду (оптимизация)
        InvokeRepeating(nameof(FindVisibleTargets), 0f, 0.2f);
    }

    void Update()
    {
        MoveToTarget();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // Находим все коллайдеры в радиусе
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInRadius.Length; i++)
        {
            Transform target = targetsInRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // Проверка угла обзора
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                // Проверка на препятствия (Raycast)
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        // Выбираем ближайшую цель, если текущей нет
        if (visibleTargets.Count > 0 && currentTarget == null)
        {
            currentTarget = GetClosestTarget(visibleTargets);
        }
    }

    void MoveToTarget()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);

            // Если подошли вплотную — "собираем" (удаляем или прячем)
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Debug.Log($"Собрал объект: {currentTarget.name}");
                // Здесь ваша логика сбора, например:
                // Destroy(currentTarget.gameObject); 
                currentTarget = null;
            }
        }
    }

    Transform GetClosestTarget(List<Transform> targets)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftRay = Quaternion.AngleAxis(-viewAngle / 2f, transform.up) * transform.forward * viewRadius;
        Vector3 rightRay = Quaternion.AngleAxis(viewAngle / 2f, transform.up) * transform.forward * viewRadius;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);

        foreach (Transform t in visibleTargets)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, t.position);
        }
    }
}
