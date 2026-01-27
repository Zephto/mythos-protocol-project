using UnityEngine;

public class MedusaBehavior : EnemyBase
{
    public TrapController[] traps;

    protected override void Update()
    {
        base.Update();

        if (playerDetected)
            ControlTraps();
    }

    void ControlTraps()
    {
        foreach (var trap in traps)
        {
            if (Random.value > 0.5f)
                trap.Activate();
            else
                trap.Deactivate();
        }
    }
}