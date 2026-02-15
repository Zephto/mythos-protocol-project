using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Walk")]
    public float speed = 5f;

    [Header("Running")]
    public bool canRun = true;
    public float runSpeed = 9f;
    public KeyCode runningKey = KeyCode.LeftShift;
    public bool IsRunning { get; private set; }

    private Rigidbody rb;
    private PlayerStatus playerStatus;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    // =====================================================
    // EXTERNAL FORCE (KNOCKBACK)
    // =====================================================
    private Vector3 externalVelocity;
    public float externalDecay = 6f;

    // =====================================================
    // INIT
    // =====================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    // =====================================================
    // METODO PUBLICO PARA QUE LOS ENEMIGOS EMPUJEN AL PLAYER
    // =====================================================
    public void AddExternalForce(Vector3 force)
    {
        externalVelocity += force;
    }

    // =====================================================
    // MOVIMIENTO
    // =====================================================
    void FixedUpdate()
    {
        // ===== PARALISIS =====
        if (playerStatus != null && playerStatus.IsParalyzed)
        {
            ApplyFinalVelocity(Vector3.zero);
            return;
        }

        // ===== RUN =====
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetSpeed = IsRunning ? runSpeed : speed;

        // multiplicador por slow
        if (playerStatus != null)
            targetSpeed *= playerStatus.moveSpeedMultiplier;

        // overrides externos
        if (speedOverrides.Count > 0)
            targetSpeed = speedOverrides[speedOverrides.Count - 1]();

        // input
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        Vector3 moveVelocity = transform.rotation * new Vector3(
            input.x * targetSpeed,
            0,
            input.y * targetSpeed
        );

        ApplyFinalVelocity(moveVelocity);
    }

    // =====================================================
    // MOVIMIENTO FINAL + EMPUJE
    // =====================================================
    void ApplyFinalVelocity(Vector3 moveVelocity)
    {
        // suavizar empuje
        externalVelocity = Vector3.Lerp(
            externalVelocity,
            Vector3.zero,
            externalDecay * Time.fixedDeltaTime
        );

        Vector3 finalVelocity =
            moveVelocity +
            externalVelocity +
            Vector3.up * rb.linearVelocity.y;

        rb.linearVelocity = finalVelocity;
    }
}
