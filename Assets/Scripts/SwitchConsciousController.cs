using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchConsciousController : MonoBehaviour {
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int ColdDown = 120;
	[SerializeField] private bool isSubShow = false;

    private int rate = 40;
    private Animation anim;
    private int count = 0;
    private bool isKeyFEnabled = true;
    private bool isKeyFDown = false;
    private int count1 = 0;
    private bool cantest = false;

    private void Start()
    {
        anim = playerController.GetComponent<Animation>();
    }

    private void Update()
	{
        if (GlobalController.gameRunning)
        {
            if (Input.GetKeyDown("f"))
            {
                isKeyFDown = true;
            }
        }
	}

    private void FixedUpdate()
    {
        if (count == ColdDown)
        {
            isKeyFEnabled = true;
            count = 0;
        }
        if (isKeyFEnabled) 
        {
            playerController.lightMode();
            if (isKeyFDown)
            {
                playerController.darkMode();
                isKeyFEnabled = false;
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
                    playerController.StopFloating();
                    playerController.CancelRigibody();
                    playerController.GetComponent<CircleCollider2D>().isTrigger = true;
                    playerController.playerTrapped = true;
                    playerController.enabled = false;
                    count1 = 0;
                    cantest = true;
                }
            }
        }
        else
        {
            if (count < ColdDown)
            {
                ++count;
            }
        }
        if (cantest)
        {
            ++count1;
            FallBack();
        }
        isKeyFDown = false;
    }

    private void FallBack()
    {
        if (playerController.isTrapped() && count1 >= rate)
        {
            anim.Play();
            if (count1 >= 2 * rate) {
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
                cantest = false;
                playerController.GetComponent<CircleCollider2D>().isTrigger = false;
                playerController.playerTrapped = false;
                playerController.enabled = true;
            }
        }
    }
}
