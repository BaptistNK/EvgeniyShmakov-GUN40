using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _maxTime;

    private NavMeshAgent _agent;
    private float _timer;
    private Animator _animator;

    void Start()
    {
        _agent=GetComponent<NavMeshAgent>();
        _timer = _maxTime;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if( _timer <= 0 )
        _agent.destination = _point.position;
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}
