using UnityEngine;

public class CharacterBehaviourExtension : MonoBehaviour
{

    private HUD_Game Hud;

	void Awake()
	{
		Hud = FindAnyObjectByType<HUD_Game>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Items"))
		{
            if (Hud.CheckInventory())
            {
                Hud.AddToInventory(other.GetComponent<Item>().GetSprite());
			    Destroy(other.gameObject);
            }

		}
	}
}
