using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 100f;
	[SerializeField] private float jumpForce = 250f;
	[SerializeField] private float floatSpeed = .02f;
	[SerializeField] private float floatHeight = .25f;
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform floatCheck;
	[SerializeField] private MainController main;

	const float checkRadius = .01f;
	private bool isGrounded;
	private bool isFloating;
	private bool jump = false;
	private float horizonMove = 0f;
	private float gravity;
	private float floatPos;
	private Rigidbody2D rb2d;
	private Vector3 groundPos;// Player's position when grounding
	private Vector3 deadPos;// Final position while player dead
	private Vector3 n_velocity = Vector3.zero;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		gravity = rb2d.gravityScale;
		floatCheck.position = groundCheck.position;
		groundPos = transform.position;
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
		// Update floatCheck position
		floatCheck.position = new Vector3(
			groundCheck.position.x,
			floatCheck.position.y,
			floatCheck.position.z
		);
		// Update ground position
		groundPos = new Vector3(transform.position.x, groundPos.y, groundPos.z);

		// Judge if ground
		isGrounded = false;
		Collider2D[] colliders =
			Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, groundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				floatCheck.position = groundCheck.position;
				groundPos = transform.position;
				floatPos = groundCheck.position.y + floatHeight;
			}
		}
		// Judge if float over ground
		if (isFloating)
		{
			StopFloating();
			colliders =
				Physics2D.OverlapCircleAll(floatCheck.position, checkRadius, groundLayer);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					KeepFloating();
				}
			}
		}
		// Handle move
		if (!main.isGameOver)
		{
			Move();
		}
	}

	private void Move()
	{
		Vector3 targetVelocity = new Vector2(horizonMove, rb2d.velocity.y);
		rb2d.velocity =
			Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref n_velocity, moveSmoothing);
		// Handle floating
		if (horizonMove != 0)
		{
			// Make or keep player floating
			if ((isGrounded || isFloating) && !jump)
			{
				KeepFloating();
			}
		}
		else
		{
			StopFloating();
		}
		// Handle jump
		if (jump)
		{
			if (isFloating)
			{
				StopFloating();
				transform.position = groundPos;
			}
			if (isGrounded)
			{
				isGrounded = false;
				rb2d.AddForce(Vector2.up * jumpForce);
				jump = false;
			}
		}
	}

	private void KeepFloating()
	{
		rb2d.gravityScale = 0f;
		isGrounded = false;
		isFloating = true;
		if (transform.position.y < floatPos)
		{
			transform.position += Vector3.up * floatSpeed;
		}
		if (transform.position.y > floatPos)
		{
			transform.position =
				new Vector3(transform.position.x, floatPos, transform.position.z);
		}
	}

	private void StopFloating()
	{
		isFloating = false;
		rb2d.gravityScale = gravity;
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
