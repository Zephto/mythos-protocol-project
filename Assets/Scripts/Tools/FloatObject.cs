using System;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
	#region Public references
	[Header("Public references")]
	public float floatHeight = 1f;
	public float speed = 1f;
	#endregion

	#region Private references
	private bool goingUp;
	private Vector3 initialPos;
	#endregion

	void Start()
	{
		initialPos = transform.position;
	}

	void Update()
	{

		float sine = Mathf.Sin(Time.time * speed);
		float offset = (sine + 1f) * 0.5f * floatHeight;

		transform.position = initialPos + Vector3.up * offset;

		// Vector3 targetPos = goingUp
		// 	? initialPos + Vector3.up * floatHeight
		// 	: initialPos;

		// transform.position = Vector3.MoveTowards(
		// 	transform.position,
		// 	targetPos,
		// 	speed * Time.deltaTime
		// );

		// if(Vector3.Distance(transform.position, targetPos) < 0.01f)
		// {
		// 	goingUp = !goingUp;
		// }
	}
}
