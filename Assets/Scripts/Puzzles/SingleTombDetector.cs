using UnityEngine;
using UnityEngine.Events;

public class SingleTombDetector : MonoBehaviour, IInteraction
{
	[Header("Public references")]
	[SerializeField] private int tombNumber;
	[SerializeField] private GameObject SphereObject;
	private bool isActivated = false;

	public UnityEvent<int> OnTombActivate = new UnityEvent<int>();

	#region Public Methods
	public int GetTombNumber() => tombNumber;
	public void ActivateTomb()
	{
		isActivated = true;
		SetVisibleSphere(false);
	}
	
	public void Reset()
	{
		isActivated = false;
		SetVisibleSphere(true);
	}

	public void SetVisibleSphere(bool set)
	{
		if(isActivated){
			SphereObject.SetActive(false);
			return;
		}

		SphereObject.SetActive(set);
	}
	#endregion

	#region Private Methods

	public void Interact(Sprite sprite)
	{
		isActivated = true;
		SetVisibleSphere(false);
		OnTombActivate?.Invoke(tombNumber);
	}
	#endregion

	#region Trigger Methods
	void OnTriggerEnter(Collider other)
	{	
		SetVisibleSphere(false);
	}

	void OnTriggerExit(Collider other)
	{
		SetVisibleSphere(true);
	}
	#endregion
}
