using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : IState
{
    private StateControllerAI _controller;
    private float _searchRadius = 5f;
    private float _patrolRadius = 20f;
    private float _searchInterval = 0.5f;
    private float _timer;
    public SearchState(StateControllerAI controller) => _controller = controller;

    public void Enter()
    {
        Debug.Log("Search object...");
        MoveToRandomPoint();
    }


    public void Exit() 
    { 
        if(_controller.agent.hasPath)
            _controller.agent.ResetPath();
    }

    public void Update()
    {
        if(!_controller.agent.pathPending && _controller.agent.remainingDistance < 0.5f)
        {
            MoveToRandomPoint();
        }
        _timer += Time.deltaTime;
        if( _timer >= _searchInterval)
        {
            _timer = 0f;
            CheckForItems();
        }
    }

    private void MoveToRandomPoint()
    {
        Vector3 _randomDirection = Random.insideUnitSphere * _patrolRadius;
        _randomDirection += _controller.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(_randomDirection, out hit, _patrolRadius, 1))
        {
            _controller.agent.SetDestination(hit.position);
        }
    }

    private void CheckForItems()
    {
        Collider[] hits = Physics.OverlapSphere(_controller.transform.position, _searchRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Item"))
            { 
                _controller.ChangeState(new CollectState(_controller, hit.transform));
                break;
            }
        }
    }
}
