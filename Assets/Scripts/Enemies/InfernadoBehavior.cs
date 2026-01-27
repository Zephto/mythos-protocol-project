using UnityEngine;

public class InfernadoBehavior : EnemyBase
{
    public Transform[] fireNodes;
    public float pushForce = 8f;

    protected override void Update()
    {
        base.Update();

        if (playerDetected)
        {
            Teleport();
            PushPlayer();
        }
    }

    void Teleport()
    {
        int index = Random.Range(0, fireNodes.Length);
        transform.position = fireNodes[index].position;
    }

    void PushPlayer()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.AddForce(dir * pushForce, ForceMode2D.Impulse);
        }
    }
}