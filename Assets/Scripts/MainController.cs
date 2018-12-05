using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public bool isGameOver = false;

	[SerializeField] private int memoryPieceNum = 3;
	[SerializeField] private DoorController doorController;

	public void playerGetPiece()
	{
		--memoryPieceNum;
	}

	public void GameOver()
	{
		isGameOver = true;
		// TODO: Game over UI
	}

	private void FixedUpdate()
	{
		if (memoryPieceNum == 0)
		{
			doorController.OpenDoor();
			--memoryPieceNum;
		}
	}
}
