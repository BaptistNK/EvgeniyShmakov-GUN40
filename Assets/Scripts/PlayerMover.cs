using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Vector3 _start = Vector3.zero;
    public Vector3 _end = Vector3.right * 5f;
    public float _speed = 1f;
    public float _delay = 1f;


    private void Start()
    {
        StartCoroutine(MoveLoop());
    }

    private System.Collections.IEnumerator MoveLoop()
    {
        bool isMovingToEnd = true;

        while (true)  
        {
            Vector3 target = isMovingToEnd ? _end : _start;
            Vector3 current = transform.position;

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                Vector3 direction = (target - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;
                yield return null;  
            }

            transform.position = target;

            yield return new WaitForSeconds(_delay);

            isMovingToEnd = !isMovingToEnd;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_start, 0.2f);
        Gizmos.DrawSphere(_end, 0.2f);
        Gizmos.DrawLine(_start, _end);
    }
}
