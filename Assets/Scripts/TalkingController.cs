using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkingController : MonoBehaviour {

	[SerializeField] private DoorController doorController;
	private int count = 0;
	private void FixedUpdate()
	{
		count++;
		if (count == 600) {
			doorController.OpenDoor();
		}
	}
}
