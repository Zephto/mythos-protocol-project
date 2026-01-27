using UnityEngine;

public class EscarchadoBehavior : EnemyBase
{
    public float freezeDuration = 2f;
    public GameObject coldFogEffect;

    protected override void Update()
    {
        base.Update();

        if (playerDetected)
        {
            FreezeEnvironment();
            ParalyzePlayer();
        }
        else
        {
            BecomeFog();
        }
    }

    void FreezeEnvironment()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (var hit in hits)
        {
            IcyDoorComponent door = hit.GetComponent<IcyDoorComponent>();
            if (door != null)
                door.Freeze();
        }
    }

    void ParalyzePlayer()
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        if (status != null)
            status.ApplyParalysis(freezeDuration);
    }

    void BecomeFog()
    {
        if (!coldFogEffect.activeSelf)
            coldFogEffect.SetActive(true);
    }
}