using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public Canvas deadCanvas;
    public Canvas tutorialCanvas;
    public Button btn;
    private bool isAlive;
    private bool inAir;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        inAir = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "memory")
        {
            collision.gameObject.SetActive(false);
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
        else if (collision.tag == "tutorial-1")
        {
            Destroy(collision.gameObject);
            if (tutorialCanvas)
            {
                tutorialCanvas.gameObject.SetActive(true);
                btn.gameObject.SetActive(true);
                tutorialCanvas.transform.GetChild(1).gameObject.SetActive(true);
            }
            //rig.velocity = Vector2.zero;
            //enabled = false;
        }
        else if (collision.tag == "tutorial-2")
        {
            Destroy(collision.gameObject);
            if (tutorialCanvas)
            {
                tutorialCanvas.gameObject.SetActive(true);
                btn.gameObject.SetActive(true);
                tutorialCanvas.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else if (collision.tag == "tutorial-3")
        {
            Destroy(collision.gameObject);
            if (tutorialCanvas)
            {
                tutorialCanvas.gameObject.SetActive(true);
                btn.gameObject.SetActive(true);
                tutorialCanvas.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }
}
