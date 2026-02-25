using UnityEngine;
using System.Collections;

public class InfernadoBehavior : EnemyBase
{
    [Header("VISUALES (ARRASTRA 'Visuals')")]
    public Transform visualsContainer;

    GameObject physicalVisual;
    GameObject elementalVisual;

    [Header("RANGO")]
    public float attackRange = 8f;
    public float attackCooldown = 2f;
    public float timeToElemental = 3f;

    [Header("FUEGO")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballForce = 12f;

    float timer;
    bool isElemental = true;
    Coroutine elementalRoutine;

    void Start()
    {
        physicalVisual = visualsContainer.Find("Physical")?.gameObject;
        elementalVisual = visualsContainer.Find("Elemental")?.gameObject;

        EnterElementalImmediate();
    }

    protected override void OnDetectPlayer()
    {
        timer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            if (isElemental)
                ExitElemental();

            if (timer <= 0)
            {
                Attack();
                timer = attackCooldown;
            }

            if (elementalRoutine != null)
            {
                StopCoroutine(elementalRoutine);
                elementalRoutine = null;
            }
        }
        else
        {
            if (!isElemental && elementalRoutine == null)
                elementalRoutine = StartCoroutine(ReturnToElemental());
        }
    }

    protected override void Attack()
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject fireball = Instantiate(
            fireballPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector3 dir = (player.position - firePoint.position).normalized;

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(dir * fireballForce, ForceMode.Impulse);
        }

        Destroy(fireball, 5f); // limpia después de 5 segundos
    }

    IEnumerator ReturnToElemental()
    {
        yield return new WaitForSeconds(timeToElemental);
        EnterElemental();
        elementalRoutine = null;
    }

    void EnterElementalImmediate()
    {
        isElemental = true;
        SetRenderersActive(physicalVisual, false);
        SetRenderersActive(elementalVisual, true);
    }

    void EnterElemental()
    {
        if (isElemental) return;

        isElemental = true;

        SetRenderersActive(physicalVisual, false);
        SetRenderersActive(elementalVisual, true);

        Debug.Log("🔥 INFERNADO vuelve a forma elemental");
    }

    void ExitElemental()
    {
        if (!isElemental) return;

        isElemental = false;

        SetRenderersActive(elementalVisual, false);
        SetRenderersActive(physicalVisual, true);

        Debug.Log("👁 INFERNADO toma forma física");
    }

    void SetRenderersActive(GameObject obj, bool state)
    {
        Renderer[] rends = obj.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in rends)
            r.enabled = state;
    }
}