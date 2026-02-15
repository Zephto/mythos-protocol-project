using UnityEngine;
using System.Collections;

public class EscarchadoBehaviour : MonoBehaviour
{
    [Header("VISUALES")]
    public GameObject physicalVisual;
    public GameObject fogVisual;

    [Header("DETECCION")]
    public float detectionDistance = 10f;
    public float timeToFog = 3f;

    [Header("ATAQUE")]
    public float attackInterval = 2f;

    [Range(0, 1)] public float slowChance = 0.7f;
    [Range(0, 1)] public float damageChance = 0.3f;

    [Header("SLOW")]
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;

    [Header("DAÑO")]
    public int damageAmount = 10;

    bool isInFog = true;
    Transform player;
    PlayerStatus playerStatus;

    Coroutine attackRoutine;
    Coroutine fogRoutine;

    // =====================================================
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        EnterFogImmediate();
    }

    // =====================================================
    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // ===== DETECT PLAYER =====
        if (dist <= detectionDistance)
        {
            if (isInFog)
                ExitFog();

            if (attackRoutine == null)
            {
                playerStatus = player.GetComponent<PlayerStatus>();
                attackRoutine = StartCoroutine(AttackLoop());
            }

            if (fogRoutine != null)
            {
                StopCoroutine(fogRoutine);
                fogRoutine = null;
            }
        }
        // ===== PLAYER LOST =====
        else
        {
            if (!isInFog && fogRoutine == null)
                fogRoutine = StartCoroutine(FogDelay());

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
        }
    }

    // =====================================================
    IEnumerator AttackLoop()
    {
        while (!isInFog && playerStatus != null)
        {
            yield return new WaitForSeconds(attackInterval);

            float roll = Random.value;

            // =========================
            // SLOW
            // =========================
            if (roll <= slowChance)
            {
                Debug.Log(
                    "ESCARCHADO APLICA SLOW → x" +
                    slowMultiplier +
                    " durante " +
                    slowDuration + "s"
                );

                playerStatus.ApplySlow(slowMultiplier, slowDuration);
            }
            // =========================
            // DAMAGE
            // =========================
            else if (roll <= slowChance + damageChance)
            {
                Debug.Log("ESCARCHADO HACE DAÑO: " + damageAmount);
                playerStatus.TakeDamage(damageAmount);
            }
        }
    }

    // =====================================================
    IEnumerator FogDelay()
    {
        yield return new WaitForSeconds(timeToFog);
        EnterFog();
        fogRoutine = null;
    }

    // =====================================================
    void EnterFogImmediate()
    {
        isInFog = true;
        physicalVisual.SetActive(false);
        fogVisual.SetActive(true);

        Debug.Log("ESCARCHADO INICIA EN FOG");
    }

    void EnterFog()
    {
        if (isInFog) return;

        isInFog = true;
        physicalVisual.SetActive(false);
        fogVisual.SetActive(true);

        Debug.Log("ESCARCHADO VUELVE A FOG");
    }

    void ExitFog()
    {
        if (!isInFog) return;

        isInFog = false;
        fogVisual.SetActive(false);
        physicalVisual.SetActive(true);

        Debug.Log("ESCARCHADO SALE DE FOG Y ATACA");
    }
}
