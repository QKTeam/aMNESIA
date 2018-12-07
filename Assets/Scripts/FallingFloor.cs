using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
	[SerializeField] private float delay = 60f;
	[SerializeField] private float checkRadius = .1f;
	[SerializeField] private bool isEntity = false;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private Transform[] playerCheck;

	private bool timing = false;
	private bool afterFalling = false;
	private BoxCollider2D m_boxCollider;
	private Rigidbody2D rb2d;

	private void Awake()
	{
		m_boxCollider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
	}

	private void Start()
	{
		CancelRigibody();
	}

	private void FixedUpdate()
	{
		if (afterFalling) return;
		if (!timing)
		{
			for (int i = 0; i < playerCheck.Length; ++i)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(
					playerCheck[i].position,
					checkRadius,
					playerLayer
				);
				for (int j = 0; j < colliders.Length; i++)
				{
					if (colliders[i].gameObject != gameObject)
					{
						timing = true;
						break;
					}
				}
				if (timing) break;
			}
		}
		if (timing)
		{
			--delay;
		}
		if (delay <= 0)
		{
			FallDown();
		}
	}

	private void CancelRigibody()
	{
		rb2d.velocity = Vector2.zero;
		rb2d.isKinematic = true;
	}
	
	private void ResumeRigibody()
	{
		rb2d.isKinematic = false;
	}

	private void FallDown()
	{
		afterFalling = true;
		ResumeRigibody();
		if (!isEntity)
		{
			m_boxCollider.isTrigger = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Edge")
		{
			Destroy(gameObject);
		}
	}
}
