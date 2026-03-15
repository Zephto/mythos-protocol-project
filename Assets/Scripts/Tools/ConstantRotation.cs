using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	public Vector3 rotationSpeed = new Vector3(0, 1, 0);

	// Update is called once per frame
	void Update()
	{
		this.transform.Rotate(rotationSpeed * Time.deltaTime);
	}
}
