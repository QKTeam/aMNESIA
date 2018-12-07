using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MenuController : MonoBehaviour 
{
	private void Start()
	{
		LoadFile();
	}

	public void NewGame()
	{
		GlobalVariable.currentLevel = 0;
		SaveFile();
		// TODO: Load CG Scene
		SceneManager.LoadScene("Level1");
	}

	public void LoadLevelSelector() 
	{
		SceneManager.LoadScene("LevelSelector");
	}

	public void QuitGame()
	{
		SaveFile();
		Application.Quit();
	}
	
	public void SaveFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		GameData data = new GameData(GlobalVariable.currentLevel);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
	}

	public void LoadFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			Debug.LogError("File not found");
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		GameData data = (GameData) bf.Deserialize(file);
		file.Close();

		GlobalVariable.currentLevel = data.level;

		Debug.Log(data.level);
	}
}
