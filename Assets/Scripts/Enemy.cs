using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настройки обзора противника")]
    public Transform playerTransform;
    public float viewRadius = 5f;
    [Range(0f, 360f)]
    public float viewAngle = 90f; 
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

        if (!IsInViewAngle(directionToPlayer))
            return;

        RaycastHit hit;
        if (Physics.Raycast(viewCenter, directionToPlayer, out hit, viewRadius))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} видит игрока!");
            }
        }
    }

    bool IsInViewAngle(Vector3 targetDirection)
    {
        Vector3 forward = transform.forward;

        float angle = Vector3.Angle(forward, targetDirection);

        return angle <= viewAngle / 2f;
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
        Gizmos.color = Color.green;
        viewCenter = transform.position + viewCenterOffset;

        Gizmos.DrawWireSphere(viewCenter, viewRadius);

        DrawViewCone();
    }

    void DrawViewCone()
    {

        Gizmos.color = Color.red;
        Vector3 leftRay = Quaternion.AngleAxis(-viewAngle / 2f, transform.up) * transform.forward * viewRadius;
        Vector3 rightRay = Quaternion.AngleAxis(viewAngle / 2f, transform.up) * transform.forward * viewRadius;

        Gizmos.DrawRay(viewCenter, leftRay);
        Gizmos.DrawRay(viewCenter, rightRay);

    }
}
