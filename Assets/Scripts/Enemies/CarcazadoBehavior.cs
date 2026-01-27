using UnityEngine;

public class CarcazadoBehavior : EnemyBase
{
    public float speed = 4f;
    public float wanderSpeed = 1.5f;

    private Vector2 wanderDirection;
    private float changeDirTime = 2f;
    private float timer;

    protected override void Update()
    {
        base.Update();

        if (playerDetected)
            ChasePlayer();
        else
            Wander();
    }

    void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        dir += Random.insideUnitCircle * 0.3f; // errático
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void Wander()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            wanderDirection = Random.insideUnitCircle.normalized;
            timer = changeDirTime;
        }

        transform.Translate(wanderDirection * wanderSpeed * Time.deltaTime);
    }
}