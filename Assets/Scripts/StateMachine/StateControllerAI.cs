using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateControllerAI : MonoBehaviour
{
    [Header("Components")]
    private IState _currentState;
    public NavMeshAgent agent;
    public Animator _animator;
    public Inventory inventory;
    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (_animator == null) _animator =GetComponent<Animator>();        
        if (inventory == null) inventory = GetComponent<Inventory>();
        ChangeState(new IdleState(this));
    }

    void Update()
    {
        _currentState?.Update();
        float speed = agent.velocity.magnitude;
        _animator.SetFloat("Speed", speed);
    }

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
