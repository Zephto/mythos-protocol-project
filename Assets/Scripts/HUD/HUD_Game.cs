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
	[SerializeField] private Image imageContainer;

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
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			currentGunSelected++;

			if(currentGunSelected >= gunBoxes.Count)
			{
				currentGunSelected = 0;
			}

			SelectGun(currentGunSelected);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			TESTcurrentLife -= 15;
			if(TESTcurrentLife <= 0) TESTcurrentLife = 0;

			UpdateHealthBar();
		}
	}

	#region Private Methods
	private void UpdateHealthBar()
	{
		healtBar.fillAmount = (float)TESTcurrentLife / (float)TESTtotalLife;

		textLife.text = string.Format("{0}/{1}", TESTcurrentLife, TESTtotalLife);
	}

	private void SelectGun(int selection)
	{
		for(int i=0; i<gunBoxes.Count; i++)
		{
			gunBoxes[i].Select( selection == i );
		}
	}

	private void UpdateGunValues()
	{
		for(int i=0; i<gunBoxes.Count; i++)
		{
			switch (i)
			{
				case 2: // Fire value
				gunBoxes[i].SetValue(totalFireBullets.ToString());
				break;

				case 3: //Ice value
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
