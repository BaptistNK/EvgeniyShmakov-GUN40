using UnityEngine;

public class RobotVacuumController : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float cleaningRadius = 1f;
    public int collectedTrash = 0;
    public AudioClip cleaningSound;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 90f;

    [Header("Raycast Settings")]
    public float rayLength = 2f;
    public LayerMask obstacleLayer;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip collisionSound;

    private Rigidbody rb;
    private Vector3 currentDirection;
    private bool isAvoidingObstacle = false;
    private float avoidanceTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomDirection();

        if (audioSource != null && moveSound != null)
        {
            audioSource.clip = moveSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        CheckForTrash();

        CheckForObstacles();

        if (!isAvoidingObstacle)
        {
            MoveForward();
        }
        else
        {
            HandleAvoidance();
        }
    }
    void CheckForTrash()
    {
        Collider[] trashColliders = Physics.OverlapSphere(transform.position, cleaningRadius, LayerMask.GetMask("Trash"));

        foreach (Collider trash in trashColliders)
        {
            if (trash.CompareTag("Trash"))
            {
                collectedTrash++;
                Debug.Log($"Собрано мусора: {collectedTrash}");

                if (audioSource != null && cleaningSound != null)
                {
                    audioSource.PlayOneShot(cleaningSound);
                }

                Destroy(trash.gameObject);
            }
        }
    }
    void MoveForward()
    {
        Vector3 movement = currentDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void CheckForObstacles()
    {
        RaycastHit hit;

        // Центральный луч
        if (Physics.Raycast(transform.position, currentDirection, out hit, rayLength, obstacleLayer))
        {
            StartAvoidance();
            PlayCollisionSound();
            return;
        }

        // Левый луч (45° влево)
        Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * currentDirection;
        if (Physics.Raycast(transform.position, leftDirection, out hit, rayLength, obstacleLayer))
        {
            currentDirection = Quaternion.Euler(0, 30, 0) * currentDirection;
            return;
        }

        // Правый луч (45° вправо)
        Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * currentDirection;
        if (Physics.Raycast(transform.position, rightDirection, out hit, rayLength, obstacleLayer))
        {
            currentDirection = Quaternion.Euler(0, -30, 0) * currentDirection;
            return;
        }
    }

    void StartAvoidance()
    {
        isAvoidingObstacle = true;
        avoidanceTimer = Random.Range(1f, 3f);
        currentDirection = Quaternion.Euler(0, Random.Range(90, 270), 0) * currentDirection;
    }

    void HandleAvoidance()
    {
        avoidanceTimer -= Time.deltaTime;

        if (avoidanceTimer <= 0)
        {
            isAvoidingObstacle = false;
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        float randomY = Random.Range(0f, 360f);
        currentDirection = Quaternion.Euler(0, randomY, 0) * Vector3.forward;
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, currentDirection * rayLength);

        Gizmos.color = Color.yellow;
        Vector3 leftDir = Quaternion.Euler(0, -45, 0) * currentDirection;
        Gizmos.DrawRay(transform.position, leftDir * rayLength);

        Vector3 rightDir = Quaternion.Euler(0, 45, 0) * currentDirection;
        Gizmos.DrawRay(transform.position, rightDir * rayLength);
    }
}
