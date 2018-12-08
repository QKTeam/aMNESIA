using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchConsciousController : MonoBehaviour {
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
    [SerializeField] private Button KeyF;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int CodeDown = 120;
	[SerializeField] private bool isSubShow = false;

    private int rate = 40;
    private Animation anim;
    private int count = 0;
    private bool isKeyFEnabled = true;
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
            if (isKeyFEnabled && KeyF) 
            {
                KeyF.interactable = true;
                if (Input.GetKeyDown("f") && isKeyFEnabled)
                {
                    KeyF.interactable = false;
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
                        count1 = 0;
                        cantest = true;
                    }
                }
            }
            else
            {
                if (KeyF)
                {
                    KeyF.interactable = false;
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
            isKeyFEnabled = true;
            count = 0;
        }
        if (cantest)
        {
            count1++;
        }
        FallBack();
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
            }
        }
    }
}
