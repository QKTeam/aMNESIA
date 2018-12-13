using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkingController : MonoBehaviour
{
	[SerializeField] private DoorController doorController;

	private int count = 0;
	private MainController main;

	private void Awake()
	{
		main = GetComponent<MainController>();
	}

	private void FixedUpdate()
	{
		if (GlobalController.gameRunning)
		{
			count++;
			if (count >= 600) {
				doorController.OpenDoor();
				main.playerGetPiece();
				enabled = false;
			}
		}
	}
}
