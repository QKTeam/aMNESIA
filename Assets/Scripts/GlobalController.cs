using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GlobalController
{
	public static int currentLevel = 0;
	public static bool gameRunning = true;

	public static void SaveFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		GameData data = new GameData(GlobalController.currentLevel);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
	}

	public static void LoadFile()
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

		GlobalController.currentLevel = data.level;

		Debug.Log(data.level);
	}
}
