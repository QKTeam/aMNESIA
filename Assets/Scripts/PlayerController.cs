using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public Canvas deadCanvas;
    public Canvas tutorialCanvas;
    private bool isAlive;
    private bool inAir;
    private int pieceNum = 3;
    private int curTutorial = 0;
    private bool jumpKeydown;
    private float horizontal;
    private int count = 0;
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
            if (curTutorial == 1) {
                if (Input.GetKeyDown("f")) {
                    curTutorial = 2;
                    tutorialCanvas.transform.GetChild(1).gameObject.SetActive(false);
                    tutorialCanvas.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
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
            enabled = false;
        }
	}

    private void FixedUpdate()
    {
        if (curTutorial == 2) ++count;
        if (count > 200)
        {
            count = 0;
            curTutorial = 3;
            tutorialCanvas.gameObject.SetActive(false);
            tutorialCanvas.transform.GetChild(2).gameObject.SetActive(false);
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
            enabled = false;
        }
        else if (collision.tag == "tutorial-1")
        {
            curTutorial = 1;
            Destroy(collision.gameObject);
            if (tutorialCanvas)
            {
                tutorialCanvas.gameObject.SetActive(true);
                tutorialCanvas.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        // else if (collision.tag == "tutorial-3")
        // {
        //     Destroy(collision.gameObject);
        //     if (tutorialCanvas)
        //     {
        //         tutorialCanvas.gameObject.SetActive(true);
        //         tutorialCanvas.transform.GetChild(3).gameObject.SetActive(true);
        //     }
        // }
    }
}
