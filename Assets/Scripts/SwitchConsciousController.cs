using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchConsciousController : MonoBehaviour {
    [SerializeField] private GameObject mask;
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int ColdDown = 80;
	[SerializeField] private bool isSubShow = false;

    private int rate = 40;
    private Animation anim;
    private int count = 0;
    private bool isKeyFEnabled = true;
    private bool isKeyFDown = false;
    private int count1 = 0;
    private bool cantest = false;

    public void SetActive()
    {
        if (!isSubShow)
        {
            subconsious.SetActive(false);
        }
        else
        {
            conscious.SetActive(false);
        }
        
    }
    

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
                    for (int i = 0; i < conscious.transform.childCount; i++)
                    {
                        Transform child = conscious.transform;
                        child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        if (child.GetChild(i).childCount > 0)
                        {
                            for (int j = 0; j < child.GetChild(i).childCount; j++)
                            {
                                child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                            }
                        }
                    }
                    for (int i = 0; i < subconsious.transform.childCount; i++)
                    {
                        Transform child = subconsious.transform;
                        child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                        if (child.GetChild(i).childCount > 0)
                        {
                            for (int j = 0; j < child.GetChild(i).childCount; j++)
                            {
                                child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                            }
                        }
                    }
                    conscious.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    subconsious.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    conscious.SetActive(true);
                    mask.GetComponent<Animation>().Play();
                }
                else
                {
                    for (int i = 0; i < conscious.transform.childCount; i++)
                    {
                        Transform child = conscious.transform;
                        child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                        if (child.GetChild(i).childCount > 0)
                        {
                            for (int j = 0; j < child.GetChild(i).childCount; j++)
                            {
                                child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                            }
                        }
                    }
                    for (int i = 0; i < subconsious.transform.childCount; i++)
                    {
                        Transform child = subconsious.transform;
                        child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        if (child.GetChild(i).childCount > 0)
                        {
                            for (int j = 0; j < child.childCount; j++)
                            {
                                child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                            }
                        }
                    }
                    conscious.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    subconsious.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    subconsious.SetActive(true);
                    mask.GetComponent<Animation>().Play();
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
