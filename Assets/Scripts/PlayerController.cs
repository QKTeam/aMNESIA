using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 130f;
	[SerializeField] private float jumpForce = 270f;
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private MainController main;

	const float checkRadius = .01f;
	private bool isGrounded;
	private bool jump = false;
	private float horizonMove = 0f;
	private Rigidbody2D rb2d;
	private Vector3 deadPos;// Final position while player dead
	private Vector3 n_velocity = Vector3.zero;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		horizonMove = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		if (main.isGameOver)
		{
			transform.position =
				Vector3.Lerp(transform.position, deadPos, Time.deltaTime * 3f);
		}
	}

	private void FixedUpdate()
	{
		isGrounded = false;
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
		if (!main.isGameOver)
		{
			Move();
			jump = false;
		}
	}

	private void Move()
	{
		Vector3 targetVelocity = new Vector2(horizonMove, rb2d.velocity.y);
		rb2d.velocity =
			Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref n_velocity, moveSmoothing);
		// Handle jump
		if (isGrounded && jump)
		{
			isGrounded = false;
			rb2d.AddForce(Vector2.up * jumpForce);
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Vesicle")
		{
			main.GameOver();
			rb2d.velocity = Vector2.zero;
			rb2d.isKinematic = true;
			deadPos = collider.transform.position;
		}
	}
}
