using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotate = new Vector3(0f, 100f,0f);
    private Rigidbody _rb;
    private bool _isRotating = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb==null)
        {
            Debug.LogError("Rigidbody is null");
            return;
        } 
        if(!_rb.isKinematic)
        {
            _rb.isKinematic = true;
        }
        if(!_isRotating)
        {
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        _isRotating = true;
        while(true)
        {
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotate * Time.deltaTime));
        yield return null;
        }
    }
   
}
