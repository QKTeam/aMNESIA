using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeConscious : MonoBehaviour {

	public GameObject surfaceConscious;
	public GameObject subConscious;
	private bool isSubDisplay = false;

	// Use this for initialization
	void Start () {
		surfaceConscious.SetActive(true);
		subConscious.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("f")) {
			isSubDisplay = !isSubDisplay;
			ChangeMode();
		}
	}

	void ChangeMode () {
		if (isSubDisplay) {
			subConscious.SetActive(true);
			surfaceConscious.SetActive(false);
		} else {
			surfaceConscious.SetActive(true);
			subConscious.SetActive(false);
		}
	}
}
