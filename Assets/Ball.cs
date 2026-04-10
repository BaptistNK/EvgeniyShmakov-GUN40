using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] GameObject floor;
    [SerializeField] float magnitude = 5f;

    private RaycastHit hit;
    private bool hasHit;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();        
    }

    void Update()
    {
        Vector3 _mousePos = GetDirectionToMouse();
        if (Input.GetMouseButton(0))
        {
            _rb.AddForce(_mousePos);
        }
    }

    Vector3 GetDirectionToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity,
            LayerMask.GetMask("floor"));
        float hitDistance = 0f;
        if (hasHit)
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Vector3 directionToMouse = targetPoint - transform.position;
            directionToMouse.y = 0f;
            if (directionToMouse != Vector3.zero)
            {
                directionToMouse.Normalize();
            }

            return directionToMouse;
        }

        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        if (hasHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, hit.point);
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        }
    }
}
