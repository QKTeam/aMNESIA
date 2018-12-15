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
    private Dictionary<string, RigidbodyInfo> storage;

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
        GameObject map;
        if (isSubShow)
        {
            map = conscious;
        }
        else
        {
            map = subconsious;
        }
        map.GetComponent<TilemapCollider2D>().enabled = true;
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Transform child = map.transform.GetChild(i);
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (child.tag == "CollisionFloor")
            {
                RestoreRigidbody(child.GetComponent<Rigidbody2D>());
            }
            if (childCollider)
            {
                childCollider.enabled = true;
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform grandchild = child.GetChild(j);
                    Collider2D grandchildCollider =
                        grandchild.GetComponent<Collider2D>();
                    if (grandchild.tag == "CollisionFloor")
                    {
                        RestoreRigidbody(grandchild.GetComponent<Rigidbody2D>());
                    }
                    if (grandchildCollider)
                    {
                        grandchildCollider.enabled = true;
                    }
                }
            }
        }
    }

    private void Start()
    {
        anim = playerController.GetComponent<Animation>();
        storage = new Dictionary<string, RigidbodyInfo>();
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
            Transform child = mapShow.transform.GetChild(i);
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer)
            {
                childRenderer.maskInteraction =
                    SpriteMaskInteraction.VisibleInsideMask;
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform grandchild = child.GetChild(j);
                    SpriteRenderer grandchildRenderer =
                        grandchild.GetComponent<SpriteRenderer>();
                    if (grandchildRenderer)
                    {
                        grandchildRenderer.maskInteraction =
                            SpriteMaskInteraction.VisibleInsideMask;
                    }
                }
            }
        }
        for (int i = 0; i < mapHide.transform.childCount; i++)
        {
            Transform child = mapHide.transform.GetChild(i);
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (childRenderer)
            {
                childRenderer.maskInteraction =
                    SpriteMaskInteraction.VisibleOutsideMask;
            }
            if (childCollider)
            {
                childCollider.enabled = false;
            }
            if (child.tag == "CollisionFloor")
            {
                ForbidRigidbody(child.GetComponent<Rigidbody2D>());
            }
            for (int j = 0; j < child.childCount; j++)
            {
                Transform grandchild = child.GetChild(j);
                SpriteRenderer grandchildRenderer =
                    grandchild.GetComponent<SpriteRenderer>();
                Collider2D grandchildCollider = grandchild.GetComponent<Collider2D>();
                if (grandchildRenderer)
                {
                    grandchildRenderer.maskInteraction =
                        SpriteMaskInteraction.VisibleOutsideMask;
                }
                if (grandchildCollider)
                {
                    grandchildCollider.enabled = false;
                }
                if (grandchild.tag == "CollisionFloor")
                {
                    ForbidRigidbody(grandchild.GetComponent<Rigidbody2D>());
                }
            }
        }
        mapShow.GetComponent<TilemapRenderer>().maskInteraction =
            SpriteMaskInteraction.VisibleInsideMask;
        mapHide.GetComponent<TilemapRenderer>().maskInteraction =
            SpriteMaskInteraction.VisibleOutsideMask;
        mapShow.SetActive(true);
    }

    private void ForbidRigidbody(Rigidbody2D rb2d)
    {
        if (rb2d)
        {
            RigidbodyInfo rbinfo;
            if (storage.TryGetValue(rb2d.name, out rbinfo))
            {
                storage[rb2d.name].velocity = rb2d.velocity;
                storage[rb2d.name].isKinematic = rb2d.isKinematic;
                storage[rb2d.name].gravityScale = rb2d.gravityScale;
                storage[rb2d.name].mass = rb2d.mass;
            }
            else {
                rbinfo = new RigidbodyInfo(
                    rb2d.velocity,
                    rb2d.isKinematic,
                    rb2d.gravityScale,
                    rb2d.mass
                );
                storage.Add(rb2d.name, rbinfo);
            }
            rb2d.velocity = Vector2.zero;
            rb2d.isKinematic = true;
        }
    }

    private void RestoreRigidbody(Rigidbody2D rb2d)
    {
        if (rb2d)
        {
            rb2d.velocity = storage[rb2d.name].velocity;
            rb2d.isKinematic = storage[rb2d.name].isKinematic;
            rb2d.gravityScale = storage[rb2d.name].gravityScale;
            rb2d.mass = storage[rb2d.name].mass;
        }
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
