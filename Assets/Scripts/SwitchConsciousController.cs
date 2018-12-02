using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchConsciousController : MonoBehaviour {
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
	[SerializeField] private bool isSubShow = false;
    [SerializeField] private PlayerController playerController;

    private float rate = 2 / 3f;
    private Animation anim;

    private void Start()
    {
        anim = playerController.GetComponent<Animation>();
    }

    private void Update()
	{
		if (Input.GetKeyDown("f"))
		{
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
                StartCoroutine(FallBack());
            }
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
    }
}
