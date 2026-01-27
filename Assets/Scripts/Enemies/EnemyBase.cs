using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    protected float currentHealth;

    [Header("Detection")]
    public float detectionRange = 5f;
    protected Transform player;

    protected bool playerDetected;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        DetectPlayer();
    }

    protected void DetectPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        playerDetected = distance <= detectionRange;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}