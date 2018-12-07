using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
	public bool isGameOver = false;
	public bool isVictory = false;

	[SerializeField] private int memoryPieceNum = 3;
	[SerializeField] private float victoryWaitTime = 100f;
	[SerializeField] private DoorController doorController;

	private float countTime = 0f;
	private int currentSceneIndex;
	private int levelOffset = -1;

	private void Awake()
	{
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	public void playerGetPiece()
	{
		--memoryPieceNum;
		if (GetAllPiece())
		{
			doorController.OpenDoor();
		}
	}

	public void GameOver()
	{
		isGameOver = true;
		// TODO: Game over UI
	}

	public bool GetAllPiece()
	{
		return memoryPieceNum == 0;
	}

	public void Victory()
	{
		isVictory = true;
		countTime = 0f;
		GlobalController.currentLevel = currentSceneIndex + 1 + levelOffset;
		GlobalController.SaveFile();
	}

	private void Update()
	{
		if (Input.GetKeyDown("r"))
		{
			SceneManager.LoadScene(currentSceneIndex);
		}
	}

	private void FixedUpdate()
	{
		if (isVictory)
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
