using UnityEngine;

public class CarcazadoBehavior : EnemyBase
{
    public float attackRange = 2f;
    public float paralyzeTime = 0.8f;
    public float pushForce = 8f;
    public float attackCooldown = 1.5f;

    private float timer;
    private PlayerStatus playerStatus;

    protected override void OnDetectPlayer()
    {
        if (player == null) return;

        if (playerStatus == null)
        {
            playerStatus = player.GetComponent<PlayerStatus>();

            if (playerStatus == null)
                playerStatus = player.GetComponentInChildren<PlayerStatus>();
        }

        timer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange && timer <= 0)
        {
            Attack();
            timer = attackCooldown;
        }
    }

    protected override void Attack()
    {
        if (playerStatus != null)
        {
            playerStatus.ApplyParalysis(paralyzeTime);
        }
        else
        {
            Debug.LogWarning("PlayerStatus no encontrado en el Player");
        }

        Rigidbody rbPlayer = player.GetComponent<Rigidbody>();
        if (rbPlayer != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            rbPlayer.AddForce(dir * pushForce, ForceMode.Impulse);
        }
    }
}