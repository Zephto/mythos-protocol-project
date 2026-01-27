using UnityEngine;

public class StoneShardsAI : MonoBehaviour
{
    public float detectionRange = 2f;
    public float attackSpeed = 6f;
    public float lifeTime = 6f;

    private Transform player;
    private bool activated;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (activated) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
            ActivateShard();
    }

    void ActivateShard()
    {
        activated = true;
        Vector2 dir = (player.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = dir * attackSpeed;
    }
}