using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    protected float currentHealth;

    protected Transform player;
    protected EnemyMovement movement;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        movement = GetComponent<EnemyMovement>();
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

    protected virtual void OnDetectPlayer() { }
    protected virtual void OnLosePlayer() { }

    protected abstract void Attack();

    protected virtual void Update()
    {
        if (!player || movement == null) return;

        if (movement.PlayerDetected)
            OnDetectPlayer();
        else
            OnLosePlayer();
    }
}
