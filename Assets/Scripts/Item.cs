using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour
{
	[Header("Public references")]
	[SerializeField] private Image imageComponent;

	
	#region Public Methods
	public Sprite GetSprite()
	{
		return imageComponent.sprite;
	}
	#endregion
}
