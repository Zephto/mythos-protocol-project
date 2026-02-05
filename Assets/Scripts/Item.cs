using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour
{
	[Header("Public references")]
	[SerializeField] private Sprite imageComponent;
	[SerializeField] private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer.sprite = imageComponent;
	}

	#region Public Methods
	public Sprite GetSprite()
	{
		return imageComponent;
	}
	#endregion
}
