using UnityEngine;
using System.Collections;

public class EscarchadoBehaviour : MonoBehaviour
{
    public GameObject enemyBody;
    public GameObject fogObject;

    public float timeToFog = 5f;
    public float slowAmount = 0.5f;
    public float slowDuration = 2f;

    bool playerInside;
    bool isInFog;
    Coroutine fogRoutine;

    void Start()
    {
        enemyBody.SetActive(true);
        fogObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerStatus ps = other.GetComponentInParent<PlayerStatus>();
        if (ps == null) return;

        playerInside = true;

        ps.ApplySlow(slowAmount, slowDuration);

        if (fogRoutine != null)
        {
            StopCoroutine(fogRoutine);
            fogRoutine = null;
        }

        if (isInFog)
            ExitFog();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerStatus>() == null) return;

        playerInside = false;

        if (!isInFog && fogRoutine == null)
            fogRoutine = StartCoroutine(FogDelay());
    }

    IEnumerator FogDelay()
    {
        yield return new WaitForSeconds(timeToFog);

        if (!playerInside)
            EnterFog();

        fogRoutine = null;
    }

    void EnterFog()
    {
        isInFog = true;
        enemyBody.SetActive(false);
        fogObject.SetActive(true);
    }

    void ExitFog()
    {
        isInFog = false;
        fogObject.SetActive(false);
        enemyBody.SetActive(true);
    }
}
