using UnityEngine;
using System.Collections;

public class CarcazadoBehavior : EnemyBase
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float attackStopTime = 0.8f;

    [Header("Damage")]
    public int damage = 10;

    [Header("Paralysis")]
    public float paralyzeTime = 1f;
    [Range(0f, 1f)] public float paralyzeChance = 0.25f;

    [Header("Push")]
    public float pushForce = 10f;
    [Range(0f, 1f)] public float pushChance = 0.35f;

    private float timer;
    private bool isAttacking;

    private PlayerStatus playerStatus;
    private Rigidbody playerRb;

    protected override void Awake()
    {
        base.Awake();

        if (player != null)
        {
            playerStatus = player.GetComponent<PlayerStatus>();
            playerRb = player.GetComponent<Rigidbody>();
        }
    }
    protected override void OnDetectPlayer()
    {
        if (player == null || isAttacking) return;

        timer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange && timer <= 0)
        {
            StartCoroutine(AttackRoutine());
            timer = attackCooldown;
        }
    }
    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (movement != null)
            movement.enabled = false;

        Attack();

        yield return new WaitForSeconds(attackStopTime);

        if (movement != null)
            movement.enabled = true;

        isAttacking = false;
    }

    protected override void Attack()
    {
        if (playerStatus == null)
        {
            Debug.LogWarning("PlayerStatus no encontrado");
            return;
        }

        playerStatus.TakeDamage(damage);
        Debug.Log("Carcazado → DAMAGE");

        float roll = Random.value;

        if (roll < paralyzeChance)
        {
            playerStatus.ApplyParalysis(paralyzeTime);
            Debug.Log("Carcazado → PARALYSIS");
            return;
        }

        if (roll < paralyzeChance + pushChance)
        {
            ApplyPush();
            Debug.Log("Carcazado → PUSH");
            return;
        }

        Debug.Log("Carcazado → SOLO DAMAGE");
    }

    void ApplyPush()
    {
        if (playerRb == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        playerRb.AddForce(dir * pushForce, ForceMode.Impulse);
    }
}
