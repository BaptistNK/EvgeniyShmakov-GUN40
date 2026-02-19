using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 1f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody is null");
            return;
        }

        transform.position = _start;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return MoveToTarget(_end);

            yield return new WaitForSeconds(_delay);

            yield return MoveToTarget(_start);

            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, target);

        if (distance < 0.01f)
            yield break;

        float duration = distance / _speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            Vector3 newPosition = Vector3.Lerp(startPosition, target, t);
            _rigidbody.MovePosition(newPosition);

            yield return new WaitForFixedUpdate();
        }

        _rigidbody.MovePosition(target);
    }
}
