using UnityEngine;

public class AlienFighterAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase }

    [Header("Behavior Settings")]
    public AIState currentState = AIState.Patrol;
    public float detectionRange = 15f;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 6f;
    public float rotationSpeed = 5f;

    [Header("Patrol Settings")]
    public Vector3 patrolArea = new Vector3(20f, 5f, 20f);
    private Vector3 targetPatrolPoint;
    public float pointReachedThreshold = 2f;

    [Header("References")]
    public Transform player;

    [Header("Damage Settings")]
    public float damageAmount = 10f;
    public float damageCooldown = 1.5f;
    private float lastDamageTime;

    [Header("Audio Settings")]
    public AudioSource droneAudioSource;
    public float maxAudioDistance = 25f;


    private void Start()
    {
        if (player == null)
        {
            // Try to find the player by tag or name
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (droneAudioSource != null)
        {
            droneAudioSource.spatialBlend = 1.0f; // Enable 3D sound
            droneAudioSource.rolloffMode = AudioRolloffMode.Linear;
            droneAudioSource.maxDistance = maxAudioDistance;
            droneAudioSource.loop = true;
            if (!droneAudioSource.isPlaying) droneAudioSource.Play();
        }

        SetNewPatrolTarget();
    }


    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            currentState = AIState.Chase;
        }
        else
        {
            currentState = AIState.Patrol;
        }

        switch (currentState)
        {
            case AIState.Patrol:
                PatrolBehavior();
                break;
            case AIState.Chase:
                ChaseBehavior();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                    Debug.Log("AlienFighter hit the player!");
                }
            }
        }
    }

    private void PatrolBehavior()
    {
        MoveTowards(targetPatrolPoint, patrolSpeed);

        if (Vector3.Distance(transform.position, targetPatrolPoint) < pointReachedThreshold)
        {
            SetNewPatrolTarget();
        }
    }

    private void ChaseBehavior()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SetNewPatrolTarget()
    {
        float x = Random.Range(-patrolArea.x, patrolArea.x);
        float y = Random.Range(2f, patrolArea.y); // Keep it off the ground
        float z = Random.Range(-patrolArea.z, patrolArea.z);
        targetPatrolPoint = transform.position + new Vector3(x, y, z);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize patrol target
        if (currentState == AIState.Patrol)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPatrolPoint);
            Gizmos.DrawWireSphere(targetPatrolPoint, 0.5f);
        }
    }
}
