using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    private bool inAir;
    private float horizontal;
    private Rigidbody2D rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
        jumpForce = 400f;
        moveSpeed = 4.0f;
        inAir = true;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !inAir)
        {
            rig.AddForce(new Vector2(0, jumpForce));
            inAir = true;
        }
        horizontal = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(horizontal * moveSpeed, rig.velocity.y);
        
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inAir = false;
    }
}
