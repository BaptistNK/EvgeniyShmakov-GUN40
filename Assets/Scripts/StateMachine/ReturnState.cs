using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IState
{
    private StateControllerAI _controller;
    private GameObject _base;
    
    public ReturnState(StateControllerAI controller) => _controller = controller;
   

    public void Enter()
    {
        _base = GameObject.FindGameObjectWithTag("Base");
        if(_base != null )
        {
            _controller.agent.SetDestination(_base.transform.position);
        }
    }

    public void Exit() => _controller.agent.ResetPath();

    public void Update()
    {
        if (_base == null) return;
        if(!_controller.agent.pathPending&&_controller.agent.remainingDistance <= _controller.agent.stoppingDistance)
        {
            Debug.Log("Complete collect");
            _controller.ChangeState(new IdleState(_controller));
        }
    }
}
