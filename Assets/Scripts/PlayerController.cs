using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 100f;
	[SerializeField] private float jumpForce = 250f;
	[SerializeField] private float floatSpeed = .02f;
	[SerializeField] private float floatHeight = .2f;
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private MainController main;

	const float checkRadius = .1f;
	private bool isGrounded;
	private bool isFloating;
	private bool jump = false;
	private float horizonMove = 0f;
	private float gravity;
	private float floatPos;
	private Rigidbody2D rb2d;
	private Vector3 deadPos;// Final position while player dead
	private Vector3 n_velocity = Vector3.zero;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		gravity = rb2d.gravityScale;
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
				floatPos = groundCheck.position.y + floatHeight;
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
			if (isGrounded || isFloating)
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
		}
		else
		{
			isFloating = false;
			rb2d.gravityScale = gravity;
		}
		// Handle jump
		if (jump)
		{
			if (isGrounded)
			{
				isGrounded = false;
				rb2d.AddForce(Vector2.up * jumpForce);
				jump = false;
			}
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
