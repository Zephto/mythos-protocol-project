using UnityEngine;
using UnityEngine.Events;

public class SingleTombDetector : MonoBehaviour, IInteraction
{
	[Header("Public references")]
	[SerializeField] private int tombNumber;
	[SerializeField] private GameObject SphereObject;
	[SerializeField] private GameObject correctLight;
	private bool isActivated = false;

	[HideInInspector] public UnityEvent<int> OnTombActivate = new UnityEvent<int>();

	void Start()
	{
		correctLight.SetActive(false);
	}

	#region Public Methods
	public int GetTombNumber() => tombNumber;
	public void ActivateTomb()
	{
		isActivated = true;
		correctLight.SetActive(true);
		SetVisibleSphere(false);
	}
	
	public void Reset()
	{
		isActivated = false;
		correctLight.SetActive(false);
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
		if(isActivated) return;

		Debug.Log("Interactue con la tumba " + tombNumber);
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
