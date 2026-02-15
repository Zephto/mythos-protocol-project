using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Movement modifiers")]
    public float moveSpeedMultiplier = 1f;

    // ===== ESTADOS =====
    private bool paralyzed;
    private bool beingPushed;

    // ===== REFERENCIAS =====
    private HUD_Game hud;
    private Rigidbody rb;

    // ======================================
    // PROPIEDADES PUBLICAS
    // ======================================
    public bool IsParalyzed => paralyzed;
    public bool IsBeingPushed => beingPushed;

    void Start()
    {
        currentHealth = maxHealth;
        hud = FindAnyObjectByType<HUD_Game>();
        rb = GetComponent<Rigidbody>();

        UpdateHUD();
    }

    // ======================================
    // DAMAGE
    // ======================================
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHUD();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHUD()
    {
        if (hud != null)
            hud.SetHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("PLAYER DEAD");
    }

    // ======================================
    // PARALYSIS
    // ======================================
    public void ApplyParalysis(float duration)
    {
        if (!paralyzed)
            StartCoroutine(ParalyzeRoutine(duration));
    }

    IEnumerator ParalyzeRoutine(float time)
    {
        paralyzed = true;
        yield return new WaitForSeconds(time);
        paralyzed = false;
    }

    // ======================================
    // SLOW (YA NO DARA ERROR)
    // ======================================
    public void ApplySlow(float multiplier, float duration)
    {
        StartCoroutine(SlowRoutine(multiplier, duration));
    }

    IEnumerator SlowRoutine(float multiplier, float duration)
    {
        moveSpeedMultiplier *= multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeedMultiplier /= multiplier;
    }

    // ======================================
    // EMPUJE FISICO REAL
    // ======================================
    public void ApplyPush(Vector3 force, float controlLockTime)
    {
        if (rb == null) return;

        StartCoroutine(PushRoutine(force, controlLockTime));
    }

    IEnumerator PushRoutine(Vector3 force, float lockTime)
    {
        beingPushed = true;

        rb.AddForce(force, ForceMode.Impulse);

        yield return new WaitForSeconds(lockTime);

        beingPushed = false;
    }
}
