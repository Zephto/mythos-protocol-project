using UnityEngine;
using System.Collections;

public class EscarchadoBehaviour : MonoBehaviour
{
    [Header("CONTENEDOR VISUAL (ARRASTRA 'Visuals')")]
    public Transform visualsContainer;

    GameObject physicalVisual;
    GameObject fogVisual;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (visualsContainer == null)
        {
            Debug.LogError("❌ Arrastra el objeto 'Visuals' al script.");
            return;
        }

        physicalVisual = visualsContainer.Find("Capsule")?.gameObject;
        fogVisual = visualsContainer.Find("FogTest")?.gameObject;

        if (physicalVisual == null || fogVisual == null)
        {
            Debug.LogError("❌ No se encontraron Capsule o FogTest dentro de Visuals.");
            return;
        }

        Debug.Log("✅ Visuales detectados correctamente.");

        EnterFogImmediate();
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // DEBUG DISTANCIA
        Debug.Log("Distancia al jugador: " + dist);

        if (dist <= detectionDistance)
        {
            if (isInFog)
                ExitFog();

            if (attackRoutine == null)
            {
                playerStatus = player.GetComponent<PlayerStatus>();
                attackRoutine = StartCoroutine(AttackLoop());
                Debug.Log("🔥 Iniciando ataque.");
            }

            if (fogRoutine != null)
            {
                StopCoroutine(fogRoutine);
                fogRoutine = null;
            }
        }
        else
        {
            if (!isInFog && fogRoutine == null)
            {
                Debug.Log("🌫 Iniciando retorno a Fog...");
                fogRoutine = StartCoroutine(FogDelay());
            }

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
                Debug.Log("🛑 Ataque detenido.");
            }
        }
    }

    IEnumerator AttackLoop()
    {
        while (!isInFog && playerStatus != null)
        {
            yield return new WaitForSeconds(attackInterval);

            float roll = Random.value;
            Debug.Log("🎲 Roll: " + roll);

            if (roll <= slowChance)
            {
                Debug.Log("❄ Aplicando slow.");
                playerStatus.ApplySlow(slowMultiplier, slowDuration);
            }
            else if (roll <= slowChance + damageChance)
            {
                Debug.Log("💥 Aplicando daño.");
                playerStatus.TakeDamage(damageAmount);
            }
        }
    }

    IEnumerator FogDelay()
    {
        yield return new WaitForSeconds(timeToFog);
        EnterFog();
        fogRoutine = null;
    }

    void EnterFogImmediate()
    {
        isInFog = true;

        physicalVisual.SetActive(false);
        fogVisual.SetActive(true);

        Debug.Log("🌫 Entra en FOG (inmediato)");
        PrintVisualState();
    }

    void EnterFog()
    {
        if (isInFog) return;

        isInFog = true;

        physicalVisual.SetActive(false);
        fogVisual.SetActive(true);

        Debug.Log("🌫 Entra en FOG");
        PrintVisualState();
    }

    void ExitFog()
    {
        if (!isInFog) return;

        isInFog = false;

        fogVisual.SetActive(false);
        physicalVisual.SetActive(true);

        Debug.Log("👁 Sale del FOG");
        PrintVisualState();
    }

    void PrintVisualState()
    {
        Debug.Log("Capsule active: " + physicalVisual.activeSelf);
        Debug.Log("Fog active: " + fogVisual.activeSelf);
    }
}