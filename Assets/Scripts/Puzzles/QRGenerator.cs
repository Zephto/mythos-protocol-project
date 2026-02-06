using System.Collections.Generic;
using System.Text;
using QRCodeShareMain;
using UnityEngine;
using UnityEngine.Events;

public class QRGenerator : MonoBehaviour, IInteraction
{
	[Header("Sphere reference")]
	[SerializeField] private GameObject SphereObject;
	
	[Header("QR references")]
	[SerializeField] private MeshRenderer targetRenderer;
	private Material runtimeMaterial;
	private Texture2D currentQRCodeGenerate = null;
	private string currentPath;

	[Header("Item sprite reference")]
	[SerializeField] private List<GameObject> visualObjects = new List<GameObject>();
	[SerializeField] private Sprite spriteRef;
	private List<GameObject> objects = new List<GameObject>();

	[HideInInspector] public UnityEvent<string> OnQRGenerated = new UnityEvent<string>();

	void Start()
	{
		//Creamos una instancia del material para no modificar el original
		runtimeMaterial = new Material(targetRenderer.material);
		targetRenderer.material = runtimeMaterial;
		targetRenderer.gameObject.SetActive(false);
		foreach(GameObject obj in visualObjects)
		{
			obj.SetActive(false);
		}
		
		currentPath = NewRandomPath();
		currentQRCodeGenerate = HelloWorldQRCode(currentPath);
	}

	#region Public Methods
	public void Interact(Sprite sprite)
	{
		if(sprite.name != spriteRef.name)
		{
			Debug.Log("There is not the correct object");
			return;
		}

		objects.Add(gameObject);
		Debug.Log("Add element, now we have: " + objects.Count);
		visualObjects[objects.Count - 1].SetActive(true);

		if(objects.Count >= 3)
		{
			Debug.Log("Imprimir QR");
			Debug.Log("Contraseña: " + currentPath);
			SetVisibleSphere(false);
			ShowQR(currentQRCodeGenerate);
		}
	}
	#endregion

	#region Private Methods
	private Texture2D HelloWorldQRCode(string content)
	{
		QRImageProperties properties = new QRImageProperties(500, 500,50);
		Texture2D QRCodeImage = QRCodeShare.CreateQRCodeImage(content, properties);
		return QRCodeImage;
	}

	private void ShowQR(Texture2D qrTexture)
	{
		if (qrTexture == null)
        {
            Debug.LogWarning("QR texture is null");
            return;
        }

		targetRenderer.gameObject.SetActive(true);
        runtimeMaterial.mainTexture = qrTexture;
		OnQRGenerated?.Invoke(currentPath);
	}

	private string NewRandomPath()
	{
		int[] numbers = new int[8];

		for(int i=0; i<8; i++)
		{
			numbers[i] = i+1;
		}

		//Fisher-Yates shuffle
		for(int i=numbers.Length - 1; i > 0; i--)
		{
			int randomIndex = Random.Range(0, i + 1);
			(numbers[i], numbers[randomIndex]) = (numbers[randomIndex], numbers[i]);
		}

		//Convertir a string
		StringBuilder sb = new StringBuilder();
		for (int i=0; i< numbers.Length; i++)
		{
			sb.Append(numbers[i]);

			if(i < numbers.Length - 1)
			{
				sb.Append("-");
			}
		}

		return sb.ToString();
	}

	private void SetVisibleSphere(bool set)
	{
		if(objects.Count < 3)
		{
			SphereObject.SetActive(set);
		}
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
