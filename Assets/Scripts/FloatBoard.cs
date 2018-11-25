using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBoard : MonoBehaviour {

	public float speed = 0.01f;
	public string moveType = "vertical";
	public float maxPos;
	public float minPos;
	public float startPos;
	public bool istoMax = true;

	private void Start()
	{
		if (moveType == "vertical")
		{
		transform.position =
			new Vector3(transform.position.x, startPos, transform.position.z);
		}
		if (moveType == "horizontal")
		{
		transform.position =
			new Vector3(startPos, transform.position.y, transform.position.z);
		}
	}

	private void FixedUpdate()
	{
		if (moveType == "vertical")
		{
			if (istoMax) transform.position += Vector3.up * speed;
			else transform.position += Vector3.down * speed;

			if (transform.position.y > maxPos)
			{
				transform.position =
					new Vector3(transform.position.x, maxPos, transform.position.z);
				istoMax = false;
			}
			if (transform.position.y < minPos)
			{
				transform.position =
					new Vector3(transform.position.x, minPos, transform.position.z);
				istoMax = true;
			}
		}
		if (moveType == "horizontal")
		{
			if (istoMax) transform.position += Vector3.right * speed;
			else transform.position += Vector3.left * speed;

			if (transform.position.x > maxPos)
			{
				transform.position =
					new Vector3(maxPos, transform.position.y, transform.position.z);
				istoMax = false;
			}
			if (transform.position.y < minPos)
			{
				transform.position =
					new Vector3(minPos, transform.position.y, transform.position.z);
				istoMax = true;
			}
		}
	}
}
