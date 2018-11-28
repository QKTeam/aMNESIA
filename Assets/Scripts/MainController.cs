using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	[SerializeField] private int memoryPieceNum = 3;

	public void playerGetPiece()
	{
		--memoryPieceNum;
	}
}
