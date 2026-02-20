using System.Collections.Generic;
using UnityEngine;

public class InvocationPlatform : MonoBehaviour
{
	#region Public references
	[Header("Public references")] 
	[SerializeField] private GameObject cylinderLight;
	[SerializeField] private GameObject particles;
	[SerializeField] private List<Pillar> pillars = new List<Pillar>();
	#endregion

	#region Private references
	private List<int> listOfPillars = new List<int>();
	
	#endregion


	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		cylinderLight.SetActive(false);
		particles.SetActive(false);

		foreach(var pillar in pillars)
		{
			pillar.OnPillarActivate.AddListener((value)=>CheckPillars(value));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	#region Private Methods
	private void CheckPillars(int pillarNumber)
	{
		if(listOfPillars.Contains(pillarNumber)) return;
		Debug.Log("Add pillar no. " + pillarNumber);

		listOfPillars.Add(pillarNumber);

		if(listOfPillars.Count >= 3)
		{
			cylinderLight.SetActive(true);
			particles.SetActive(true);
		}
	}
	#endregion
}
