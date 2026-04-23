using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float angleRotation = 79f;
    [SerializeField] private float rotationSpeed = 70f;
    [SerializeField] private float _rayLength;
    [SerializeField] private bool isRotate = false;
    private Coroutine movementCleaner;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if(!isRotate)
        {
            StartMovement();
        }
        Vector3 origin = transform.position;
        Vector3 _frontRay = transform.forward;
        Vector3 _leftRay = -transform.right;
        Vector3 _rightRay = transform.right;

        RaycastHit _frontHit, _leftHit, _rightHit;

        bool frontHit = Physics.Raycast(origin, _frontRay, out _frontHit, _rayLength);
        Debug.DrawRay(origin, _frontRay * _rayLength, Color.red);
        
        if(frontHit)
        {
            StopMovement();
            float _leftLength = 0;
            float _rightLength = 0;
            
            if(Physics.Raycast(origin, _leftRay, out _leftHit))
            {
                _leftLength = _leftHit.distance;
            }
            if(Physics.Raycast(origin, _rightRay, out _rightHit))
            {
                _rightLength = _rightHit.distance;
            }
            if (_leftLength <= _rightLength)
            {
                if (angleRotation < 0)
                {
                    angleRotation *= -1;
                }
                
            }
            else
            {
                if (angleRotation > 0) 
                {
                    angleRotation *= -1;
                }
            }

        }
    }

    public void StartMovement()
    {
        if(movementCleaner==null)
        {
            movementCleaner = StartCoroutine(CleanerMove());
            Debug.Log("Start move");
        }
    }

    public void StopMovement()
    {
        if (movementCleaner != null) 
        {
            StopCoroutine(CleanerMove());
            movementCleaner = null;
            Debug.Log("Stop move");
        }
    }
    IEnumerator CleanerMove()
    {
        while (true)
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
            yield return null;
        }
    }

    /*IEnumerator CleanerRotation()
    {
        Debug.Log("Start rotation");
        isRotate = true;
        float currentAngle = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angleRotation, 0);

        while (currentAngle < angleRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            currentAngle += rotationStep;
            if (currentAngle > angleRotation)
                currentAngle = angleRotation;

            float t = currentAngle / angleRotation;
            transform.rotation = Quaternion.Slerp(startRotation,targetRotation, t);
            yield return null;
        }
        isRotate = false;
        Debug.Log("Stop rotation");
    }*/
}
