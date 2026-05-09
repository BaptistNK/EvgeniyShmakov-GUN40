using UnityEngine;
using UnityEngine.AI;

public class BotBrain : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewRadius = 5f;
    public Transform currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();    
    }
}
