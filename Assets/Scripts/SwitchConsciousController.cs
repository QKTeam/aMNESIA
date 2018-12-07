using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchConsciousController : MonoBehaviour {
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int CodeDown = 120;
	[SerializeField] private bool isSubShow = false;

    private float rate = 2 / 3f;
    private Animation anim;
    private int count = 0;
    private bool enable = true;

    private void Start()
    {
        anim = playerController.GetComponent<Animation>();
    }

    private void Update()
	{
        if (GlobalController.gameRunning)
        {
            if (Input.GetKeyDown("f") && enable)
            {
                enable = false;
                if (isSubShow)
                {
                    conscious.SetActive(true);
                    subconsious.SetActive(false);
                }
                else
                {
                    subconsious.SetActive(true);
                    conscious.SetActive(false);
                }
                isSubShow = !isSubShow;
                if (playerController.isTrapped())
                {
                    playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    playerController.GetComponent<Rigidbody2D>().isKinematic = true;
                    playerController.GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(FallBack());
                }
            }
        }
	}

    private void FixedUpdate()
    {
        if (count < CodeDown)
        {
            count++;
        }
        else
        {
            enable = true;
            count = 0;
        }
    }

    IEnumerator FallBack()
    {
        yield return new WaitForSeconds(rate);
        if (playerController.isTrapped())
        {
            anim.Play();
            yield return new WaitForSeconds(rate);
            if (isSubShow)
            {
                conscious.SetActive(true);
                subconsious.SetActive(false);
            }
            else
            {
                subconsious.SetActive(true);
                conscious.SetActive(false);
            }
            isSubShow = !isSubShow;
        }
        playerController.GetComponent<Collider2D>().enabled = true;
        playerController.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
