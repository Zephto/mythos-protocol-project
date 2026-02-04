using System.Collections.Generic;
using UnityEngine;

public class QRGenerator : MonoBehaviour, IInteraction
{
	[SerializeField] private Sprite spriteRef;
	private List<GameObject> objects = new List<GameObject>();

	#region Public Methods
	public void AddObject()
	{
		
	}

	public void Interact(Sprite sprite)
	{
		if(sprite.name != spriteRef.name)
		{
			Debug.Log("There is not the correct object");
			return;
		}

		objects.Add(gameObject);
		Debug.Log("Add element, now we have: " + objects.Count);


		if(objects.Count >= 3)
		{
			Debug.Log("Imprimir QR");
		}
	}
	#endregion

}
