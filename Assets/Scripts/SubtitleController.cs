using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleController : MonoBehaviour
{
	[SerializeField] private Text[] subtitle;

	private int writtenSerialNum = 0;
	private int shownSerialNum = 1;
	private float m_delay = -1f;

	public void Showtext(string text, float delaySecond)
	{
		subtitle[writtenSerialNum].text = text;
		if (delaySecond > 0)
		{
			m_delay = delaySecond / Time.fixedDeltaTime;
		}
		// Hide and show subtitle
		subtitle[shownSerialNum].gameObject.SetActive(false);
		subtitle[writtenSerialNum].gameObject.SetActive(true);
		// Swap serial number
		int temp = shownSerialNum;
		shownSerialNum = writtenSerialNum;
		writtenSerialNum = temp;
	}

	public void Cleartext()
	{
		subtitle[shownSerialNum].gameObject.SetActive(false);
	}

	private void FixedUpdate()
	{
		// Update delay time
		if (m_delay > 0)
		{
			--m_delay;
		}
		// Hide subtitle while delay equal 0
		if (m_delay == 0)
		{
			Cleartext();
		}
	}
}
