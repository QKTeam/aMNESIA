using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
	[SerializeField] private GameObject GameMenu;

	public void ResumeGame()
    {
        GameMenu.SetActive(false);
    }

    public void ReturnMainMenu()
    {
        GameMenu.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}