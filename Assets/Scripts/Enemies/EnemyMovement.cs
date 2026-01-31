using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float detectionRange = 10f;
    public float wanderRadius = 6f;
    public float directionChangeTime = 2f;

    public bool PlayerDetected { get; private set; }

    private Rigidbody rb;
    private Transform player;
    private Vector3 wanderTarget;
    private float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);
        PlayerDetected = distance <= detectionRange;

        Vector3 targetDir;

        if (PlayerDetected)
        {
            targetDir = (player.position - transform.position).normalized;
        }
        else
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                wanderTarget = transform.position + Random.insideUnitSphere * wanderRadius;
                wanderTarget.y = transform.position.y;
                timer = directionChangeTime;
            }

            targetDir = (wanderTarget - transform.position).normalized;
        }

        rb.linearVelocity = new Vector3(
            targetDir.x * moveSpeed,
            rb.linearVelocity.y,
            targetDir.z * moveSpeed
        );
    }

    void OnCollisionStay(Collision collision)
    {
        // Evita atorarse con objetos
        if (!collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushOut = collision.contacts[0].normal;
            rb.AddForce(pushOut * 2f, ForceMode.Impulse);
        }
    }
}