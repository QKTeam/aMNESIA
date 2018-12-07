using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelController : MonoBehaviour 
{
	public void LoadLevel(int num) 
	{
		SceneManager.LoadScene("Level" + num);
	}
}
