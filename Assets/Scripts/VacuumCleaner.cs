using System.Collections;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float angleRotation = 79f;
    [SerializeField] private float rotationSpeed = 70f;
    [SerializeField] private float _rayLength = 1.5f;

    private Coroutine movementCleaner;
    private bool isRotating = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }
    }

    private void Update()
    {
        Vector3 origin = transform.position;
        Vector3 _frontRay = transform.forward;
        Vector3 _leftRay = -transform.right;
        Vector3 _rightRay = transform.right;
        RaycastHit _frontHit;
        RaycastHit _leftHit;
        RaycastHit _rightHit;

        bool frontHit = Physics.Raycast(origin, _frontRay, out _frontHit, _rayLength);
        bool leftHit = Physics.Raycast(origin, _leftRay, out _leftHit, _rayLength);
        bool rightHit = Physics.Raycast(origin, _rightRay, out _rightHit, _rayLength);
        Debug.DrawRay(origin, _frontRay * _rayLength, frontHit ? Color.red : Color.green);
        Debug.DrawRay(origin, _leftRay * _rayLength, frontHit ? Color.red : Color.green);
        Debug.DrawRay(origin, _rightRay * _rayLength, frontHit ? Color.red : Color.green);
        if (!frontHit && !isRotating)
        {
            StartMovement();
        }
        else if (frontHit && !isRotating) 
        {
            StopMovement();
            StartCoroutine(RotateWhenHit());
        }        
    }

    public void StartMovement()
    {
        if (movementCleaner == null)
        {
            movementCleaner = StartCoroutine(CleanerMove());
            Debug.Log("Start move");
        }
    }

    public void StopMovement()
    {
        if (movementCleaner != null)
        {
            StopCoroutine(movementCleaner);
            movementCleaner = null;
            Debug.Log("Stop move");
        }
    }

    IEnumerator CleanerMove()
    {
        while (true)
        {
            Vector3 movement = transform.forward * _speed * Time.deltaTime;
            _rb.MovePosition(_rb.position + movement);
            yield return null;
        }
    }

    IEnumerator RotateWhenHit()
    {
        isRotating = true;
        Debug.Log("start rotation");
        float rotationAngle = Random.value > 0.5f ? angleRotation : -angleRotation;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0);

        float totalRotationTime = rotationSpeed > 0 ? Mathf.Abs(rotationAngle) / rotationSpeed : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < totalRotationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / totalRotationTime;
            t = Mathf.SmoothStep(0f, 1f, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        Debug.Log("Stop rotation");
        transform.rotation = targetRotation;
        isRotating = false;
    }
}
