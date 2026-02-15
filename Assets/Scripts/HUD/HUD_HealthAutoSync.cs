using UnityEngine;
using System.Reflection;

public class HUD_HealthAutoSync : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HUD_Game hud;
    [SerializeField] private MonoBehaviour playerHealthScript;

    [Header("Player health variable names (exact name in Player script)")]
    [SerializeField] private string currentHealthField = "currentHealth";
    [SerializeField] private string maxHealthField = "maxHealth";

    FieldInfo playerCurrentHealth;
    FieldInfo playerMaxHealth;

    FieldInfo hudCurrentLife;
    FieldInfo hudTotalLife;

    MethodInfo updateHealthBarMethod;

    void Start()
    {
        if (hud == null || playerHealthScript == null)
        {
            Debug.LogError("HUD_HealthAutoSync → Missing references");
            return;
        }

        // ===== PLAYER HEALTH =====
        System.Type playerType = playerHealthScript.GetType();

        playerCurrentHealth = playerType.GetField(currentHealthField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        playerMaxHealth = playerType.GetField(maxHealthField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (playerCurrentHealth == null || playerMaxHealth == null)
        {
            Debug.LogError("HUD_HealthAutoSync → Player health fields not found. Check names EXACTLY.");
            return;
        }

        // ===== HUD PRIVATE FIELDS =====
        System.Type hudType = hud.GetType();

        hudCurrentLife = hudType.GetField("TESTcurrentLife", BindingFlags.Instance | BindingFlags.NonPublic);
        hudTotalLife = hudType.GetField("TESTtotalLife", BindingFlags.Instance | BindingFlags.NonPublic);

        updateHealthBarMethod = hudType.GetMethod("UpdateHealthBar", BindingFlags.Instance | BindingFlags.NonPublic);

        if (hudCurrentLife == null || hudTotalLife == null || updateHealthBarMethod == null)
        {
            Debug.LogError("HUD_HealthAutoSync → HUD fields not found.");
        }
    }

    void Update()
    {
        if (playerCurrentHealth == null) return;

        int current = (int)playerCurrentHealth.GetValue(playerHealthScript);
        int max = (int)playerMaxHealth.GetValue(playerHealthScript);

        hudCurrentLife.SetValue(hud, current);
        hudTotalLife.SetValue(hud, max);

        updateHealthBarMethod.Invoke(hud, null);
    }
}
