using UnityEngine;

[System.Serializable]
public class Subtitle
{
	public enum SubtitleType
	{
		Start,// The text when scene starting
		BindObject,// The text when colliding object
	}
	public string content;
	public bool isForever;
	public float delay;
	public int nextIndex = -1;
	public SubtitleType type;
	public GameObject bindObject;
}
