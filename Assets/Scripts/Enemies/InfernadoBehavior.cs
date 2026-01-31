using UnityEngine;

public class InfernadoBehavior : EnemyBase
{
    public Transform[] fireNodes;
    public float pushForce = 6f;
    public float attackRange = 7f;
    public float attackCooldown = 2f;

    float timer;

    protected override void OnDetectPlayer()
    {
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
        Rigidbody rbPlayer = player.GetComponent<Rigidbody>();
        if (rbPlayer != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            rbPlayer.AddForce(dir * pushForce, ForceMode.Impulse);
        }

        if (fireNodes != null && fireNodes.Length > 0)
        {
            Transform node = fireNodes[Random.Range(0, fireNodes.Length)];
            transform.position = node.position;
        }
    }
}
