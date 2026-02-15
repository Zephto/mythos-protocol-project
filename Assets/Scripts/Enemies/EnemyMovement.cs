using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float acceleration = 8f;

    [Header("Detection")]
    public float detectionRange = 10f;

    [Header("Wander")]
    public float wanderRadius = 6f;
    public float directionChangeTime = 2f;

    [Header("Grounding")]
    public float stickToGroundForce = 12f; // lo pega al suelo
    public float maxSlopeAngle = 60f;

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
        rb.linearDamping = 3f; // elimina vibración física
    }

    void FixedUpdate()
    {
        if (!player) return;

        DetectPlayer();
        Move();
        StickToGround();
    }

    void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        PlayerDetected = distance <= detectionRange;
    }

    void Move()
    {
        Vector3 targetDir;

        if (PlayerDetected)
        {
            targetDir = (player.position - transform.position).normalized;
        }
        else
        {
            timer -= Time.fixedDeltaTime;

            if (timer <= 0f)
            {
                wanderTarget = transform.position + Random.insideUnitSphere * wanderRadius;
                wanderTarget.y = transform.position.y;
                timer = directionChangeTime;
            }

            targetDir = (wanderTarget - transform.position).normalized;
        }

        // velocidad deseada (solo horizontal)
        Vector3 desiredVelocity = new Vector3(
            targetDir.x * moveSpeed,
            0f,
            targetDir.z * moveSpeed
        );

        // conservar velocidad vertical real
        Vector3 currentVelocity = rb.linearVelocity;

        // suavizado (MUY IMPORTANTE para no brincar)
        Vector3 smoothVelocity = Vector3.Lerp(
            new Vector3(currentVelocity.x, 0, currentVelocity.z),
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            smoothVelocity.x,
            currentVelocity.y,
            smoothVelocity.z
        );
    }

    void StickToGround()
    {
        // fuerza constante hacia abajo para evitar micro saltos
        rb.AddForce(Vector3.down * stickToGroundForce, ForceMode.Acceleration);
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // empuje horizontal solamente (NO vertical)
            Vector3 normal = collision.contacts[0].normal;
            normal.y = 0f;
            normal.Normalize();

            rb.AddForce(normal * 2f, ForceMode.Impulse);
        }
    }
}
