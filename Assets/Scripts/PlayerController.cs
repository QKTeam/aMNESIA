using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playerTrapped = false;

    [SerializeField] private float moveSpeed = 100f;
	[SerializeField] private float jumpForce = 250f;
	[SerializeField] private float floatSpeed = .02f;
	[SerializeField] private float floatHeight = .25f;
	[SerializeField] private float staticWaitTime = 120f;// While player no input
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Sprite lightStatus;
	[SerializeField] private Sprite darkStatus;
	[SerializeField] private Transform groundCheck;
    [SerializeField] private Transform topCheck;
    [SerializeField] private Transform leftCheck;
    [SerializeField] private Transform rightCheck;
	[SerializeField] private Transform floatCheck;
	[SerializeField] private MainController main;
	[SerializeField] private SubtitleController subtitleController;

	const float checkRadius = .01f;

	private bool isGrounded;
	private bool isFloating;
	private bool jump = false;
	private bool inWindZone = false;
	private bool rigibodyLocked = false;
	private float horizonMove = 0f;
	private float gravity;
	private float floatPos;
	private string currentState = "Normal";
	private float waitTime = 0f;// While player no input
	private Rigidbody2D rb2d;
	private Vector3 groundPos;// Player's position when grounding
	private Vector3 finalPos;// Final position while player dead
	private Vector3 finalScale;
	private Vector3 n_velocity = Vector3.zero;
	private Vector2 pauseVelocity = Vector2.zero;
	private GameObject windZone;
	private Animator animator;

    public void KeepFloating()
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

    public void StopFloating()
    {
        isFloating = false;
        rb2d.gravityScale = gravity;
    }

    public void CancelRigibody()
    {
        pauseVelocity = rb2d.velocity;
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
        rigibodyLocked = true;
    }

    public void ResumeRigibody()
    {
        rb2d.velocity = pauseVelocity;
        rb2d.isKinematic = false;
        rigibodyLocked = false;
    }

    public void lightMode()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("CloseEye"))
		{
			animator.Play("OpenEye");
		}
	}

	public void darkMode()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CloseEye"))
		{
			animator.Play("CloseEye");
		}
	}

    public bool isTrapped()
    {
        bool lefttrapped = false;
        bool righttrapped = false;
        bool toptrapped = false;
        Collider2D[] leftcolliders =
            Physics2D.OverlapCircleAll(leftCheck.position, checkRadius, groundLayer);
        Collider2D[] rightcolliders =
            Physics2D.OverlapCircleAll(rightCheck.position, checkRadius, groundLayer);
        Collider2D[] topcolliders =
            Physics2D.OverlapCircleAll(topCheck.position, checkRadius, groundLayer);
        for (int i = 0; i < leftcolliders.Length; i++)
        {
            if (leftcolliders[i].gameObject != gameObject)
            {
                lefttrapped = true;
            }
        }
        for (int i = 0; i < rightcolliders.Length; i++)
        {
            if (rightcolliders[i].gameObject != gameObject)
            {
                righttrapped = true;
            }
        }
        for (int i = 0; i < topcolliders.Length; i++)
        {
            if (topcolliders[i].gameObject != gameObject)
            {
                toptrapped = true;
            }
        }
        if ((isGrounded && toptrapped) || (lefttrapped && righttrapped))
        {
            return true;
        }
        return false;
    }

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		gravity = rb2d.gravityScale;
		floatCheck.position = groundCheck.position;
		groundPos = transform.position;
	}

	private void Update()
	{
		if (GlobalController.gameRunning)
		{
			horizonMove = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}
			if (rigibodyLocked && !playerTrapped)
			{
				ResumeRigibody();
			}
		}
		else
		{
			horizonMove = 0;
			jump = false;
			if (main.gameStopStatus == "GameOver")
			{
				transform.position = Vector3.Lerp(
					transform.position,
					finalPos,
					Time.deltaTime * 3f
				);
				transform.localScale = Vector3.Lerp(
					transform.localScale,
					finalScale,
					Time.deltaTime
				);
			}
			else if (main.gameStopStatus == "Victory")
			{
				transform.position = Vector3.Lerp(
					transform.position,
					finalPos,
					Time.deltaTime * 3f
				);
				transform.localScale = Vector3.Lerp(
					transform.localScale,
					finalScale,
					Time.deltaTime
				);
			}
			else if (main.gameStopStatus == "Pause") {
				if (!rigibodyLocked)
				{
					CancelRigibody();
				}
			}
		}
	}

	private void FixedUpdate()
	{
		// Calculate wait
		if (!jump && horizonMove == 0)
		{
			++waitTime;
		}
		else
		{
			waitTime = 0f;
		}
		// Judge player static
		if (waitTime >= staticWaitTime)
		{
			StopFloating();
		}
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
        // Handle wind force
        if (inWindZone)
        {
            jump = false;
            WindController wind = windZone.GetComponent<WindController>();
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(wind.direction * wind.strength);
        }
		// Handle Player animation
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CloseEye"))
		{
			if (isGrounded || isFloating)
			{
				if (horizonMove > 0 && currentState != "Rightward")
				{
					animator.Play("Rightward");
					currentState = "Rightward";
				}
				else if (horizonMove < 0 && currentState != "Leftward")
				{
					animator.Play("Leftward");
					currentState = "Leftward";
				}
				else if (horizonMove == 0 && currentState != "Normal")
				{
					animator.Play("Normal");
					currentState = "Normal";
				}
			}
			else if (horizonMove == 0)
			{
				if (rb2d.velocity.y > 0 && currentState != "Upward")
				{
					animator.Play("Upward");
					currentState = "Upward";
				}
				else if (rb2d.velocity.y < 0 && currentState != "Downward")
				{
					animator.Play("Downward");
					currentState = "Downward";
				}
			}
		}
        // Handle move
		Move();
		jump = false;
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
		// Handle jump
		if (jump)
		{
			if (isGrounded || isFloating)
			{
				isGrounded = false;
				StopFloating();
				rb2d.AddForce(Vector2.up * jumpForce);
			}
		}
	}


	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Vesicle")
		{
			main.GameOver();
			CancelRigibody();
			finalPos = collider.transform.position;
			finalScale = new Vector3(0, 0, 0);
		}
		if (collider.tag == "WindZone")
		{
			inWindZone = true;
			windZone = collider.gameObject;
            StopFloating();
		}
		if (collider.tag == "Door")
		{
			if (main.GetAllPiece())
			{
				main.Victory();
				CancelRigibody();
				finalPos = collider.transform.position;
				finalScale = new Vector3(0, 0, 0);
			}
		}
		subtitleController.CollisionEvent(collider);
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Door" && main.gameStopStatus != "Victory")
		{
			if (main.GetAllPiece())
			{
				main.Victory();
				CancelRigibody();
				finalPos = collider.transform.position;
				finalScale = new Vector3(0, 0, 0);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "WindZone")
		{
			inWindZone = false;
		}
	}
}
