using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_GunBox : MonoBehaviour
{
	[Header("Public references")]
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private TextMeshProUGUI textIndicator;

	#region Public Methods
	public void SetValue(string set)
	{
		textIndicator.text = set;
	}

	public void Select(bool set)
	{
		if (set)
		{
			canvasGroup.alpha = 1;
		}
		else
		{
			canvasGroup.alpha = 0.2f;
		}
	}
	#endregion
}
