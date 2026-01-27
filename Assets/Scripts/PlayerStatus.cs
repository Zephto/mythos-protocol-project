using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool paralyzed;

    public void ApplyParalysis(float duration)
    {
        if (!paralyzed)
            StartCoroutine(Paralyze(duration));
    }

    IEnumerator Paralyze(float time)
    {
        paralyzed = true;
        yield return new WaitForSeconds(time);
        paralyzed = false;
    }

    public void ApplySlow(float amount, float duration)
    {
        StartCoroutine(Slow(amount, duration));
    }

    IEnumerator Slow(float amount, float time)
    {
        moveSpeed *= amount;
        yield return new WaitForSeconds(time);
        moveSpeed /= amount;
    }
}
