using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AILocomotion : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _maxTime;
    private NavMeshAgent _agent;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _agent=GetComponent<NavMeshAgent>();
        _timer = _maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0 )
        {
            _agent.destination = _playerTransform.position;
        }
        
    }
}
