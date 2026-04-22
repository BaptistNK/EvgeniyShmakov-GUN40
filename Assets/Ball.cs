using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField] float _inpulseForce;
    public Rigidbody _rb;
    private RaycastHit hit;
    public bool isInPlay = false;
    Vector3 _targetImpulse;
    private int currentThrowInFrame = 1;
    private FrameManager frameManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();        
        frameManager = FindObjectOfType<FrameManager>();
        
        isInPlay = true;
    }

    public void Launch(Vector3 force)
    {
        if (isInPlay)
        {
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (isInPlay && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                _targetImpulse = (hit.point - transform.position).normalized;
                _targetImpulse.y = 0;
                _targetImpulse.Normalize();

                Vector3 _totalImpulse = _targetImpulse * _inpulseForce;
                Launch(_totalImpulse);
                isInPlay = false; 
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if(collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log($"Шар столкнулся с кеглей");
            HandlePinCollision(collision);
        }
        else */if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log($"запуск корутины");
            StartCoroutine(WaitForBallStop());            
        }
        if (!isInPlay) return;
    }
    private void HandlePinCollision(Collision collision)
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb == null) return;
        }

        if (!isInPlay) return;

        if (frameManager == null || GameManager.Instance == null)
        {
            Debug.LogError("Зависимости не инициализированы!");
            return;
        }

        
        if (_rb.velocity.magnitude < 0.1f)
        {
            ProcessBallStop(collision);
        }
        else
        {
            StartCoroutine(WaitForBallStop());
        }
    }


    private void ProcessBallStop(Collision collision)
    {
        Debug.Log("ProcessBallStop вызван. Сбито кеглей: " + GameManager.Instance.GetFallenPinCounts());

        isInPlay = false;

        int fallenCount = GameManager.Instance.GetFallenPinCounts();
        Debug.Log($"Сбито кеглей: {fallenCount}");

        frameManager.RegisterThrow(fallenCount);
    }


    public void ResetForNextThrow(int throwNumber)
    {
        isInPlay = true;
        currentThrowInFrame = throwNumber; 
        Debug.Log($"Шар готов к броску №{currentThrowInFrame}");
    }


    private IEnumerator WaitForBallStop()
    {
        Debug.Log("WaitForBallStop: запуск корутины ожидания остановки шара.");
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f); 

        if (_rb.velocity.magnitude < 0.1f && !isInPlay)
        {
            Debug.Log("WaitForBallStop: шар остановился. Вызов ProcessBallStop.");

            ProcessBallStop(null);
        }
        else
        {
            Debug.Log($"WaitForBallStop: шар ещё движется. Скорость: {_rb.velocity.magnitude}. Повторная проверка.");
            StartCoroutine(WaitForBallStop());
        }
    }    
}
