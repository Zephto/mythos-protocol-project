using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviourExtension : MonoBehaviour
{

	#region public variables
	[SerializeField] private int currentGunSelection = 0;
	[SerializeField] private List<GameObject> GunsObjects; //Aqui puedes cambiar el Gameobject por el tipo de arma para accionarla por una funcion generica
	[SerializeField] private Animator gunAnimator;
	[SerializeField] private ParticleSystem fireGunPs;
	#endregion

	#region private variables
    private HUD_Game Hud;
	private bool isMouseLPressed;
	private IEnumerator currentGunCoroutine;
	#endregion

	void Awake()
	{
		Hud = FindAnyObjectByType<HUD_Game>();
		currentGunCoroutine = null;
	}

	void Start()
	{
		Hud.OnGunChange.AddListener((value) => ChangeGun(value));
		Hud.OnShoot.AddListener(()=>ShootGun());

		foreach(GameObject gun in GunsObjects)
		{
			gun.SetActive(false);
		}
	
	}

	void Update()
	{
		isMouseLPressed = Input.GetMouseButtonDown(0);
	}

	#region Private Methods
	private void ChangeGun(int value)
	{
		currentGunSelection = value;
		if(currentGunCoroutine != null){
			StopCoroutine(currentGunCoroutine);
			currentGunCoroutine = null;
		}

		currentGunCoroutine = ChangeGunCoroutine(value);
		StartCoroutine(currentGunCoroutine);
	}

	private void ShootGun()
	{
		gunAnimator.SetTrigger("SHOOT");
		fireGunPs.Play();
	}
	#endregion

	#region Coroutines
	private IEnumerator ChangeGunCoroutine(int value)
	{
		currentGunSelection = value - 1;

		gunAnimator.SetTrigger("OUT");
		yield return new WaitForSeconds(0.6f);
		
		if(currentGunSelection >= 0)
		{
			for(int i=0; i<GunsObjects.Count; i++) {
				GunsObjects[i].SetActive(i == currentGunSelection);
			}
		}
		else
		{
			foreach(GameObject gun in GunsObjects)
			{
				gun.SetActive(false);
			}
		}

		gunAnimator.SetTrigger("IN");
	}
	#endregion

	#region Trigger detection
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Items"))
		{
            if (!Hud.CheckInventory()) {
                Hud.AddToInventory(other.GetComponent<Item>().GetSprite());
			    Destroy(other.gameObject);
            }
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(!isMouseLPressed) return;


		if(currentGunSelection != 0)
		{
			Debug.Log("No se puede interactuar si tienes un arma");
			return;
		}

		if (other.TryGetComponent<IInteraction>(out var interaction))
		{
			Debug.Log("Check inventory: " + Hud.CheckInventory());
			if (Hud.CheckInventory())
            {
				interaction.Interact(Hud.UseInventory());
			}
			else
			{
				interaction.Interact(null);
			}
		}
	}
	#endregion
}
