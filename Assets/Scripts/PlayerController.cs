using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 2.5f;
	[SerializeField] private float jumpForce = 300f;
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;

	const float checkRadius = .01f;
	private bool isGrounded;
	private bool isAlive = true;
	private float moveInput;
	private float jumpInput;
	private Rigidbody2D rb2d;
	private Vector3 n_velocity = Vector3.zero;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		moveInput = Input.GetAxisRaw("Horizontal");
		jumpInput = Input.GetAxisRaw("Vertical");
	}

	private void FixedUpdate()
	{
		// Judge if ground
		Collider2D[] colliders =
			Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, groundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
			}
		}
		// Handle move
		if (isAlive)
		{
			Move();
		}
	}

	private void Move()
	{
		Vector3 targetVelocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
		rb2d.velocity =
			Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref n_velocity, moveSmoothing);
		// Handle jump
		if (isGrounded && jumpInput > 0)
		{
			isGrounded = false;
			rb2d.AddForce(Vector2.up * jumpForce);
		}
	}
}
