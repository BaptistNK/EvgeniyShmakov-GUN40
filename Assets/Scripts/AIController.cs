using System;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Animator _animator;
    private float idleTimer = 0f;
    public float searchRadius = 5f;
    public LayerMask itemLayer;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Idle"))
        {
            UpdateIdle();
        }
        else if (stateInfo.IsName("Search"))
        {
            UpdateSearch();
        }
    }

    private void UpdateIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > 5f)
        {
            idleTimer = 0;
            _animator.SetTrigger("startSearch");
        }
    }

    private void UpdateSearch()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        if (hitColliders.Length > 0)
        {
            _animator.SetTrigger("foundItem");
        }
    }

    public void OnCollectFinished()
    {
        _animator.SetTrigger("workDone");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        Gizmos.DrawWireSphere(transform.position,searchRadius);
    }
}
