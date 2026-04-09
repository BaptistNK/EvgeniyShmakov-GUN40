using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настройки обзора противника")]
    public Transform playerTransform;
    public float viewRadius = 5f;
    public Vector3 viewCenterOffset = Vector3.zero;
    private Vector3 viewCenter;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        CalculateViewArea();
    }

    void CalculateViewArea()
    {
        viewCenter = transform.position + viewCenterOffset;
        float distanceToPlayer = Vector3.Distance(viewCenter, playerTransform.position);

        if (distanceToPlayer > viewRadius)
            return;
        Vector3 directionToPlayer = (playerTransform.position - viewCenter).normalized;

        RaycastHit hit;
        if (Physics.Raycast(viewCenter, directionToPlayer, out hit, viewRadius))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} видит игрока!");
            }
        }
    }

    public Vector3 GetViewCenter()
    {
        return viewCenter;
    }

    public float GetViewRadius()
    {
        return viewRadius;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       // Vector3 viewCenter = GetViewCenter();
        //float viewRadius = GetViewRadius();

        Gizmos.DrawWireSphere(viewCenter, viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + 10 * transform.forward);
    }
}
