using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    private bool inAir;
    private bool jumpKeydown;
    private float horizontal;
    private Rigidbody2D rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
        jumpForce = 450f;
        moveSpeed = 4.0f;
        inAir = true;
        jumpKeydown = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            jumpKeydown = true;
            if (!inAir)
            {
                rig.AddForce(new Vector2(0, jumpForce));
                inAir = true;
            }
        }
        horizontal = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(horizontal * moveSpeed, rig.velocity.y);
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (jumpKeydown)
        {
            jumpKeydown = false;
            return;
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            float angle = Vector2.Angle(Vector2.up, contact.normal);
            if (angle < 90) inAir = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "memory")
        {
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "dead")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
