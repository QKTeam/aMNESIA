using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGsound : MonoBehaviour
{
	private static BGsound instance = null;
	public static BGsound Instance {
		get { return instance; }
	}

	private void Awake()
	{
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
