using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotate = new Vector3(0f, 300f,0f);
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody is null");
            return;
        } 
            _rb.isKinematic = true;
        StartCoroutine(Rotate());        
    }

    private IEnumerator Rotate()
    {
        while(true)
        {
            Quaternion rotation = Quaternion.Euler(_rotate*Time.deltaTime);
            _rb.MoveRotation(_rb.rotation * rotation);
        yield return null;
        }
    }
   
}
