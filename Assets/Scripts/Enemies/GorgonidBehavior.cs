using UnityEngine;

using UnityEngine;

public class GorgonidBehavior : EnemyBase
{
    [Header("Debuff")]
    public float slowAmount = 0.5f;
    public float debuffDuration = 3f;

    [Header("Stone Shards")]
    public StoneShardsAI stoneShardPrefab;
    public Transform[] shardSpawnPoints;
    public float shardCooldown = 5f;

    private float shardTimer;

    protected override void Update()
    {
        base.Update();

        if (!playerDetected) return;

        ApplyDebuff();
        HandleStoneShards();
    }

    void ApplyDebuff()
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        if (status != null)
            status.ApplySlow(slowAmount, debuffDuration);
    }

    void HandleStoneShards()
    {
        shardTimer -= Time.deltaTime;
        if (shardTimer <= 0)
        {
            SpawnShard();
            shardTimer = shardCooldown;
        }
    }

    void SpawnShard()
    {
        Transform spawn = shardSpawnPoints[Random.Range(0, shardSpawnPoints.Length)];
        Instantiate(stoneShardPrefab, spawn.position, Quaternion.identity);
    }
}