using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public bool isGameOver = false;

	[SerializeField] private int memoryPieceNum = 3;

	public void playerGetPiece()
	{
		--memoryPieceNum;
	}

	public void GameOver()
	{
		isGameOver = true;
		// TODO: Game over UI
	}
}
