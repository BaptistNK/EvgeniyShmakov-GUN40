using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private float _fallThresholdAngle = 30f;
    [SerializeField] private float _fallThresholdDistance = 0.3f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private bool isFallen = false;

    void Start()
    {
        _originalRotation = transform.rotation;
        _originalPosition = transform.position;
    }

    void Update()
    {
        if(!isFallen)
        {
            CheckIfFallen();
        }
    }

    private void CheckIfFallen()
    {
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        float distanceMoved = Vector3.Distance(transform.position, _originalPosition);
        if(tiltAngle>_fallThresholdAngle || distanceMoved>_fallThresholdDistance)
        {
            isFallen = true;
            GameManager.Instance.RegisterFallenPin(this);
            Destroy(gameObject, 2);
        }
    }
    public void ResetPin()
    {
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        isFallen = false;
    }
}
