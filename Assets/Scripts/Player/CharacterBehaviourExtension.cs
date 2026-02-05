using UnityEngine;

public class CharacterBehaviourExtension : MonoBehaviour
{

    private HUD_Game Hud;
	private bool isMouseLPressed;

	void Awake()
	{
		Hud = FindAnyObjectByType<HUD_Game>();
	}

	void Update()
	{
		isMouseLPressed = Input.GetMouseButtonDown(0);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Items"))
		{
            if (!Hud.CheckInventory())
            {
                Hud.AddToInventory(other.GetComponent<Item>().GetSprite());
			    Destroy(other.gameObject);
            }
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(!isMouseLPressed) return;

		if (other.TryGetComponent<IInteraction>(out var interaction))
		{
			Debug.Log("Check inventory: " + Hud.CheckInventory());
			if (Hud.CheckInventory())
            {
				interaction.Interact(Hud.UseInventory());
            }
		}
	}
}
