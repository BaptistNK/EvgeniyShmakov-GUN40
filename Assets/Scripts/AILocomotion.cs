using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AILocomotion : MonoBehaviour
{
    [SerializeField] private Transform _item;
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
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            //_agent.destination = _item.position;
        }
        //_animator.SetFloat("Search", _agent.velocity.magnitude);
    }
}
