using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleController : MonoBehaviour
{
	[SerializeField] private Text[] subtitle;
	[SerializeField] private GameObject panel;
	[SerializeField] private Subtitle[] startText;
	[SerializeField] private Subtitle[] bindText;
	[SerializeField] private MainController main;

	private int writtenIndex = 0;
	private int shownIndex = 1;
	private float m_delay = -1f;
	private Subtitle currentText;

	private void Start()
	{
		if (startText.Length > 0)
		{
			currentText = startText[0];
			currentText.shown = true;
			Showtext(currentText.content, currentText.delay);
		}
	}

	public void CollisionEvent(Collider2D collider)
	{
		if (collider.tag == "Door" && main.gameStopStatus != "Victory")
		{
			return;
		}
		for (int i = 0; i < bindText.Length; ++i)
		{
			if (bindText[i].bindObject && !bindText[i].shown)
			{
				if (bindText[i].bindObject == collider.gameObject)
				{
					currentText = bindText[i];
					currentText.shown = true;
					Showtext(currentText.content, currentText.delay);
				}
			}
		}
	}

	public void Showtext(string text, float delaySecond)
	{
		panel.SetActive(true);
		subtitle[writtenIndex].text = text;
		if (delaySecond > 0)
		{
			m_delay = delaySecond / Time.fixedDeltaTime;
		}
		// Hide and show subtitle
		subtitle[shownIndex].gameObject.SetActive(false);
		subtitle[writtenIndex].gameObject.SetActive(true);
		// Swap serial number
		int temp = shownIndex;
		shownIndex = writtenIndex;
		writtenIndex = temp;
	}

	public void Cleartext()
	{
		if (currentText.nextIndex >= 0)
		{
			switch (currentText.type.ToString())
			{
				case "Start":
					currentText = startText[currentText.nextIndex];
					break;
				case "BindObject":
					currentText = bindText[currentText.nextIndex];
					break;
				default:
					break;
			}
			currentText.shown = true;
			Showtext(currentText.content, currentText.delay);
		}
		else
		{
			subtitle[shownIndex].gameObject.SetActive(false);
			panel.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
		// Update delay time
		if (m_delay > 0)
		{
			--m_delay;
		}
		// Hide subtitle while delay equal 0
		if (m_delay == 0 && !currentText.isForever)
		{
			Cleartext();
		}
	}
}
