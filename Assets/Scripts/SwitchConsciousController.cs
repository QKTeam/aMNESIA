using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchConsciousController : MonoBehaviour {
	[SerializeField] private GameObject conscious;
	[SerializeField] private GameObject subconsious;
	[SerializeField] private bool isSubShow = false;

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
		}
	}
}
