using UnityEngine;

public class CollectState : IState
{
    private StateControllerAI _controller;
    private Transform _target;
    public CollectState(StateControllerAI controller, Transform target)
    {
        _controller = controller;
        _target = target;
    }
    public void Enter()
    {
        Debug.Log("Go to object..");
        _controller.agent.SetDestination(_target.position);
    }
    public void Update()
    {
        if(_target==null)
        {
            _controller.ChangeState(new IdleState(_controller));
            return;
        }
        if (!_controller.agent.pathPending && _controller.agent.remainingDistance < 0.5f)
        {
            PickUp(); 
        }
    }

    private void PickUp()
    {
        _controller._animator.SetTrigger("Lift");
        _controller.inventory.AddItem();
        Object.Destroy(_target.gameObject);
        if(_controller.inventory.IsFull())
        {
            _controller.ChangeState(new ReturnState(_controller));
        }
        else
        {
            _controller.ChangeState(new IdleState(_controller));
        }
    }
    public void Exit() 
    { 
        if(_controller.agent.hasPath)
            _controller.agent.ResetPath();
    }   
}
