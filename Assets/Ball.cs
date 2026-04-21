using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!isInPlay) return;

        if(collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log($"Шар столкнулся с кеглей");
            HandlePinCollision(collision);
        }
        else if(collision.gameObject.CompareTag("Wall"))
        {
            if(_rb.velocity.magnitude < 0.1f)
            {
                StartCoroutine(WaitForBallStop());
            }
        }
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

        Debug.Log($"Обработка столкновения с кеглей. Скорость шара: {_rb.velocity.magnitude:F3}");

        
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
        yield return new WaitForSeconds(1.5f); // Ждём 1.5 с после столкновения

        if (_rb.velocity.magnitude < 0.1f && isInPlay)
        {
            ProcessBallStop(null); // Передаём null, так как столкновение уже произошло
        }
    }


    private IEnumerator CheckStopAndRegisterThrow()
    {
        yield return new WaitForSeconds(0.5f);

        if (_rb.velocity.magnitude < 0.1f && isInPlay)
        {
            isInPlay = false;

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager не инициализирован!");
                yield break;
            }

            int fallenCount = GameManager.Instance.GetFallenPinCounts();
            Debug.Log($"Сбито кеглей: {fallenCount}");

            if (frameManager != null && !frameManager.IsFrameComplete())
            {
                frameManager.RegisterThrow(fallenCount);
            }
            else
            {
                Debug.LogWarning("Фрейм уже завершён. Бросок игнорирован");
            }

            if (!frameManager.IsGameOver())
            {
                GameManager.Instance.ResetAllPins();
            }
        }
    }
}
