using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSubtitle : MonoBehaviour {

	[SerializeField] private GameObject Present;
	[SerializeField] private GameObject Next;

	public void ChangeSubtitle()
	{
		Present.SetActive(false);
		if (Next.name == "End")
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
