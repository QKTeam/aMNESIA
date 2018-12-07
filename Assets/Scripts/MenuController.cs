using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MenuController : MonoBehaviour 
{
	public GameObject MenuButton;

	private void Start()
	{
		GlobalController.LoadFile();
		if (GlobalController.currentLevel == 0) 
		{
			MenuButton.SetActive(false);
		}
	}

	public void NewGame()
	{
		GlobalController.currentLevel = 0;
		GlobalController.SaveFile();
		// TODO: Load CG Scene
		SceneManager.LoadScene("Level1");
	}

	public void LoadLevelSelector() 
	{
		SceneManager.LoadScene("LevelSelector");
	}

	public void QuitGame()
	{
		GlobalController.SaveFile();
		Application.Quit();
	}
}
