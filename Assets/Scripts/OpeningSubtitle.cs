using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSubtitle : MonoBehaviour {

	[SerializeField] private GameObject Present;
	[SerializeField] private GameObject Next;
	// Use this for initialization
	public void ChangeSubtitle()
	{
		Present.SetActive(false);
		if (Next.name == "Empty")
		{
			SceneManager.LoadScene("TalkingScene");
		}
		else if (Next.name == "End")
		{
			SceneManager.LoadScene("Menu");
		}
		else
		{
			Next.SetActive(true);
			Next.GetComponent<Animator>().Play("Subtitles");
		}
	}
}
