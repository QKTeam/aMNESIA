using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
	public bool isGameOver = false;
	public bool isVictory = false;

	[SerializeField] private int memoryPieceNum = 3;
	[SerializeField] private float victoryWaitTime = 200f;
	[SerializeField] private DoorController doorController;

	private float countTime = 0f;

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
			LoadNextScene();
		}
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
