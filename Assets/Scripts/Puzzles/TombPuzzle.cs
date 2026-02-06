using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TombPuzzle : MonoBehaviour
{
	[Header("Public references")]
	[SerializeField] private QRGenerator qrGenerator;
	[SerializeField] private GameObject prize;
	[SerializeField] private List<SingleTombDetector> tombDetectors = new List<SingleTombDetector>();

	private int[] sequence;
	private int currentIndex;

	void Start()
	{
		prize.gameObject.SetActive(false);
		foreach(SingleTombDetector tomb in tombDetectors)
		{
			tomb.SetVisibleSphere(false);
			tomb.OnTombActivate.AddListener((value)=> CheckAnswer(value));
		}

		qrGenerator.OnQRGenerated.AddListener((value) => ActivateTombs(value));
	}

	#region Private Methods
	private void ActivateTombs(string path)
	{
		foreach(SingleTombDetector tomb in tombDetectors)
		{
			tomb.SetVisibleSphere(true);
		}

		sequence = path
			.Split('-')
			.Select(int.Parse)
			.ToArray();
	}
	
	private void CheckAnswer(int currentTomb)
	{
		if(sequence[currentIndex] == currentTomb)
		{
			currentIndex++;
			foreach(SingleTombDetector tomb in tombDetectors)
			{
				if(tomb.GetTombNumber() == currentTomb)
				{
					tomb.ActivateTomb();
				}
			}

			if(currentIndex >= sequence.Length)
			{
				Debug.Log("Codigo correcto, soltar premio!!");

				prize.gameObject.SetActive(true);
				foreach(SingleTombDetector tomb in tombDetectors)
				{
					tomb.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			Debug.Log("Codigo erroneo, reiniciando...");
			foreach(SingleTombDetector tomb in tombDetectors)
			{
				tomb.Reset();
			}
			currentIndex = 0;
		}
	}
	#endregion
}
