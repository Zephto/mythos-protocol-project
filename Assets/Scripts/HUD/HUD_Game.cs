using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD_Game : MonoBehaviour
{
	[Header("Health references")]
	[SerializeField] private Image healtBar;
	[SerializeField] private TextMeshProUGUI textLife;

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
	}

	#region Private Methods
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
