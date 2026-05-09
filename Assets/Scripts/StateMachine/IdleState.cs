using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IState
{
    private float _timer;
    private const float _delay = 5f;
    private StateControllerAI _controller;
    public IdleState(StateControllerAI controller) => _controller = controller;
    public void Enter()
    { 
        Debug.Log("Enter to Idle");
        _timer = 0f;
    }
    public void Exit() { }


    // Update is called once per frame
    public void Update()
    {
        _timer += Time.deltaTime;
        if(_timer>=_delay)
        {
            SwitchState();
        }
    }
    void SwitchState()
    {
        _controller.ChangeState(new SearchState(_controller));
    }
}
