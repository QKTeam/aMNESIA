using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour 
{
	[SerializeField] int levelNum;

	private Button m_button;

	private void Awake()
	{
		m_button = GetComponent<Button> ();
	}
	private void Start()
	{
		if (levelNum > GlobalController.currentLevel)
		{
			m_button.interactable = false;
		}
	}

	public void LoadLevel() 
	{
		SceneManager.LoadScene("Level" + levelNum);
	}
}
