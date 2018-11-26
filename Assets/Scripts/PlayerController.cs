using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public Canvas deadCanvas;
    private bool isAlive;
    private bool inAir;
    private int pieceNum = 3;
    private bool jumpKeydown;
    private float horizontal;
    private Rigidbody2D rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
        jumpForce = 450f;
        moveSpeed = 4.0f;
        isAlive = true;
        inAir = true;
        jumpKeydown = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAlive)
        {
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
        if (pieceNum == 0) {
            rig.velocity = Vector3.zero;
            rig.isKinematic = true;
            if (deadCanvas)
            {
                Text wintext =
                    deadCanvas.transform.GetChild(1).gameObject.GetComponent<Text>();
                wintext.text = "You Win!";
                deadCanvas.gameObject.SetActive(true);
            } 
        }
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
            --pieceNum;
        }
        else if (collision.tag == "dead")
        {
            isAlive = false;
            rig.velocity = Vector3.zero;
            rig.isKinematic = true;
            if (deadCanvas)
            {
                deadCanvas.gameObject.SetActive(true);
            }
        }
    }
}
