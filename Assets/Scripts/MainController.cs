using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
	public bool isGameOver = false;
	public bool isVictory = false;
	public bool isPause = false;

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
		doorController.OpenDoor();
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
		if (GameMenu.activeInHierarchy == false)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GameMenu.SetActive(true);
				isPause = true;
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GameMenu.SetActive(false);
				isPause = false;
			}			
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
