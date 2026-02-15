using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD_Game : MonoBehaviour
{
    [Header("Health references")]
    [SerializeField] private Image healtBar;
    [SerializeField] private TextMeshProUGUI textLife;
    private int TESTcurrentLife;
    private int TESTtotalLife;

    [Header("Guns selector references")]
    ///<summary>
    /// Each space represent one type of gun:
    /// 1. None | Interaction
    /// 2. Normal
    /// 3. Fire
    /// 4. Ice
    ///</summary>
    [SerializeField] private List<HUD_GunBox> gunBoxes = new List<HUD_GunBox>();
    private int currentGunSelected;
    private int totalFireBullets;
    private int totalIceBullets;

    [Header("Inventory references")]
    [SerializeField] private Image inventoryImage;
    private GameObject inventoryObject;


    private void Start()
    {
        TESTtotalLife = 100;
        TESTcurrentLife = TESTtotalLife;
        UpdateHealthBar();

        currentGunSelected = 0;
        totalFireBullets = 10;
        totalIceBullets = 25;
        SelectGun(currentGunSelected);
        UpdateGunValues();

        inventoryObject = null;
        inventoryImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentGunSelected++;

            if (currentGunSelected >= gunBoxes.Count)
            {
                currentGunSelected = 0;
            }

            SelectGun(currentGunSelected);
        }
    }

    #region Public Methods

    public void SetHealth(int current, int total)
    {
        TESTcurrentLife = current;
        TESTtotalLife = total;
        UpdateHealthBar();
    }

    public void AddToInventory(Sprite iSprite)
    {
        inventoryImage.sprite = iSprite;
        inventoryImage.gameObject.SetActive(true);
    }

    public Sprite UseInventory()
    {
        inventoryImage.gameObject.SetActive(false);
        Sprite savedSprite = inventoryImage.sprite;
        inventoryImage.sprite = null;
        return savedSprite;
    }

    public bool CheckInventory()
    {
        return inventoryImage.gameObject.activeSelf;
    }

    #endregion


    #region Private Methods

    private void UpdateHealthBar()
    {
        if (healtBar != null)
            healtBar.fillAmount = (float)TESTcurrentLife / (float)TESTtotalLife;

        if (textLife != null)
            textLife.text = string.Format("{0}/{1}", TESTcurrentLife, TESTtotalLife);
    }

    private void SelectGun(int selection)
    {
        for (int i = 0; i < gunBoxes.Count; i++)
        {
            gunBoxes[i].Select(selection == i);
        }
    }

    private void UpdateGunValues()
    {
        for (int i = 0; i < gunBoxes.Count; i++)
        {
            switch (i)
            {
                case 2:
                    gunBoxes[i].SetValue(totalFireBullets.ToString());
                    break;

                case 3:
                    gunBoxes[i].SetValue(totalIceBullets.ToString());
                    break;

                default:
                    gunBoxes[i].SetValue("-");
                    break;
            }
        }
    }

    #endregion
}
