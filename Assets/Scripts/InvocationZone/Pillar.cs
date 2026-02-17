using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pillar : MonoBehaviour, IInteraction
{
	#region Public references
	[Header("Public references")]
	[SerializeField] private int pillarNumber;
	//[SerializeField] private Item itemReference;
	[SerializeField] private GameObject correctLight;
	[SerializeField] private GameObject sphereObject;
	[SerializeField] private SpriteRenderer itemSprite; 
	[SerializeField] private List<Item> itemReferences;
	#endregion

	#region Private references
	private bool isActivated = false;
	#endregion

	[HideInInspector] public UnityEvent<int> OnPillarActivate = new UnityEvent<int>();

	void Start()
	{
		correctLight.SetActive(false);
		itemSprite.gameObject.SetActive(false);
		correctLight.SetActive(false);
	}

	private void SetVisibleSphere(bool set)
	{
		if (isActivated)
		{
			sphereObject.SetActive(false);
			return;
		}

		sphereObject.SetActive(set);
	}

	public void Interact(Sprite sprite)
	{
		if(isActivated) return;

		Debug.Log("Este pilar se ha activadoooo");
		isActivated = true;
		
		itemSprite.sprite = sprite;
		itemSprite.gameObject.SetActive(true);

		correctLight.SetActive(true);
		SetVisibleSphere(false);
		OnPillarActivate?.Invoke(pillarNumber);
	}

	void OnTriggerEnter(Collider other)
	{
		SetVisibleSphere(false);
	}

	void OnTriggerExit(Collider other)
	{
		SetVisibleSphere(true);
	}
}
