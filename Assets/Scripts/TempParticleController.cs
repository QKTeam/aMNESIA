using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParticleController : MonoBehaviour
{
	[SerializeField] private float delay = 2f;

	private void Awake()
	{
		delay = delay / Time.fixedDeltaTime;
	}

	private void FixedUpdate()
	{
		--delay;
		if (delay <= 0)
		{
			Destroy(gameObject);
		}
	}
}
