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

    public void HideMap()
    {
        if (isSubShow)
        {
            conscious.SetActive(false);
        }
        else
        {
            subconsious.SetActive(false);
        }
        
    }

    public void RestoreCollider()
    {
        conscious.GetComponent<TilemapCollider2D>().enabled = true;
        subconsious.GetComponent<TilemapCollider2D>().enabled = true;
        for (int i = 0; i < conscious.transform.childCount; i++)
        {
            Transform child = conscious.transform;
            if (child.GetChild(i).GetComponent<Collider2D>())
            {
                child.GetChild(i).GetComponent<Collider2D>().enabled = true;
                if (child.GetChild(i).childCount > 0)
                {
                    for (int j = 0; j < child.GetChild(i).childCount; j++)
                    {
                        if (child.GetChild(i).GetChild(j).GetComponent<Collider2D>())
                        {
                            child.GetChild(i).GetChild(j).GetComponent<Collider2D>().enabled = true;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < subconsious.transform.childCount; i++)
        {
            Transform child = subconsious.transform;
            if (child.GetChild(i).GetComponent<Collider2D>())
            {
                child.GetChild(i).GetComponent<Collider2D>().enabled = true;
                if (child.GetChild(i).childCount > 0)
                {
                    for (int j = 0; j < child.GetChild(i).childCount; j++)
                    {
                        if (child.GetChild(i).GetChild(j).GetComponent<Collider2D>())
                        {
                            child.GetChild(i).GetChild(j).GetComponent<Collider2D>().enabled = true;
                        }
                    }
                }
            }
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
                    subconsious.GetComponent<TilemapCollider2D>().enabled = false;
                    MaskPrehandle(conscious, subconsious);
                    mask.GetComponent<Animation>().Play();
                }
                else
                {
                    conscious.GetComponent<TilemapCollider2D>().enabled = false;
                    MaskPrehandle(subconsious, conscious);
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

    private void MaskPrehandle(GameObject mapShow, GameObject mapHide)
    {
        for (int i = 0; i < mapShow.transform.childCount; i++)
        {
            Transform child = mapShow.transform;
            if (child.GetChild(i).GetComponent<SpriteRenderer>())
            {
                child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction =
                    SpriteMaskInteraction.VisibleInsideMask;
                if (child.GetChild(i).childCount > 0)
                {
                    for (int j = 0; j < child.GetChild(i).childCount; j++)
                    {
                        if (child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                        {
                            child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction =
                                SpriteMaskInteraction.VisibleInsideMask;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < mapHide.transform.childCount; i++)
        {
            Transform child = mapHide.transform;
            if (child.GetChild(i).GetComponent<SpriteRenderer>())
            {
                child.GetChild(i).GetComponent<SpriteRenderer>().maskInteraction =
                    SpriteMaskInteraction.VisibleOutsideMask;
                if (child.GetChild(i).childCount > 0)
                {
                    for (int j = 0; j < child.GetChild(i).childCount; j++)
                    {
                        if (child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                        {
                            child.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().maskInteraction =
                                SpriteMaskInteraction.VisibleOutsideMask;
                        }
                    }
                }
            }
            if (child.GetChild(i).GetComponent<Collider2D>())
            {
                child.GetChild(i).GetComponent<Collider2D>().enabled = false;
                if (child.GetChild(i).childCount > 0)
                {
                    for (int j = 0; j < child.GetChild(i).childCount; j++)
                    {
                        if (child.GetChild(i).GetChild(j).GetComponent<Collider2D>())
                        {
                            child.GetChild(i).GetChild(j).GetComponent<Collider2D>().enabled = false;
                        }
                    }
                }
            }
        }
        mapShow.GetComponent<TilemapRenderer>().maskInteraction =
            SpriteMaskInteraction.VisibleInsideMask;
        mapHide.GetComponent<TilemapRenderer>().maskInteraction =
            SpriteMaskInteraction.VisibleOutsideMask;
        mapShow.SetActive(true);
    }

    private void FallBack()
    {
        if (playerController.isTrapped() && count1 >= rate)
        {
            anim.Play();
            if (count1 >= 2 * rate) {
                if (isSubShow)
                {
                    subconsious.GetComponent<TilemapCollider2D>().enabled = false;
                    MaskPrehandle(conscious, subconsious);
                    mask.GetComponent<Animation>().Play();
                }
                else
                {
                    conscious.GetComponent<TilemapCollider2D>().enabled = false;
                    MaskPrehandle(subconsious, conscious);
                    mask.GetComponent<Animation>().Play();
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
