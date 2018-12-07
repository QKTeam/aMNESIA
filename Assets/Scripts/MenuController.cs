using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
	[SerializeField] private Button new_btn; 
	[SerializeField] private Button continue_btn;
	[SerializeField] private Button quit_btn;

	private void Start()
	{
		GlobalController.LoadFile();
		if (GlobalController.currentLevel == 0) 
		{
			continue_btn.gameObject.SetActive(false);
		}
		// Add event listener
		new_btn.onClick.AddListener(NewGame);
		continue_btn.onClick.AddListener(LoadLevelSelector);
		quit_btn.onClick.AddListener(QuitGame);
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
