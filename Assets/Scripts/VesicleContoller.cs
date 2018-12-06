using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesicleContoller : MonoBehaviour
{
	[SerializeField] private string direction = "vertical";
	[SerializeField] private float isToMax = 1f;
	[SerializeField] private float floatSpeed = .6f;
	[SerializeField] private float maxPos;
	[SerializeField] private float minPos;

	private void FixedUpdate()
	{
		if (direction == "vertical")
		{
			float diffY = floatSpeed * isToMax * Time.fixedDeltaTime;
			if (transform.position.y + diffY > maxPos)
			{
				diffY = maxPos - transform.position.y;
				isToMax = -1f;
			}
			else if (transform.position.y + diffY < minPos)
			{
				diffY = minPos - transform.position.y;
				isToMax = 1f;
			}
			transform.position += Vector3.up * diffY;
		}
		else if (direction == "horizontal")
		{
			float diffX = floatSpeed * isToMax * Time.fixedDeltaTime;
			if (transform.position.x + diffX > maxPos)
			{
				diffX = maxPos - transform.position.x;
				isToMax = -1f;
			}
			else if (transform.position.x + diffX < minPos)
			{
				diffX = minPos - transform.position.x;
				isToMax = 1f;
			}
			transform.position += Vector3.right * diffX;
		}
	}
}
