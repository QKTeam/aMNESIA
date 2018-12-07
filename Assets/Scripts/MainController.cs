﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
	public string gameStopStatus = "";// The reason of game stop

	[SerializeField] private int memoryPieceNum = 3;
	[SerializeField] private float victoryWaitTime = 100f;
	[SerializeField] private DoorController doorController;
	[SerializeField] private GameObject GameMenu;

	private float countTime = 0f;
	private int currentSceneIndex;
	private int levelOffset = -1;

	private void Awake()
	{
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	private void Start() {
		GameMenu.SetActive(false);
	}

	public void playerGetPiece()
	{
		--memoryPieceNum;
	}

	public void GameOver()
	{
		GlobalController.gameRunning = false;
		gameStopStatus = "GameOver";
		// TODO: Game over UI
	}

	public bool GetAllPiece()
	{
		return memoryPieceNum == 0;
	}

	public void Victory()
	{
		GlobalController.gameRunning = false;
		gameStopStatus = "Victory";
		doorController.OpenDoor();
		countTime = 0f;
		GlobalController.currentLevel = currentSceneIndex + 1 + levelOffset;
		GlobalController.SaveFile();
	}

	private void Update()
	{
		if (GlobalController.gameRunning)
		{
			if (Input.GetKeyDown("r"))
			{
				SceneManager.LoadScene(currentSceneIndex);
			}
			if (!GameMenu.activeInHierarchy)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					GameMenu.SetActive(true);
					GlobalController.gameRunning = false;
					gameStopStatus = "Pause";
				}
			}
		}
		else if (GameMenu.activeInHierarchy) {
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GameMenu.SetActive(false);
				GlobalController.gameRunning = true;
			}	
		}
	}

	private void FixedUpdate()
	{
		if (gameStopStatus == "Victory")
		{
			++countTime;
		}
		if (countTime == victoryWaitTime)
		{
			enabled = false;
			SceneManager.LoadScene(currentSceneIndex + 1);
		}
	}
}
