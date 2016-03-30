using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpeechBubbleReply : MonoBehaviour {

	public Text replyText;
	public Sprite regularBG;
	public Sprite selectedBG;

	private Image bg;

	void Awake()
	{
		bg = GetComponent<Image> ();
	}

	public string text { get { return replyText.text; } set { replyText.text = value; }}

	public void Select()
	{
		bg.sprite = selectedBG;
	}

	public void Unselect()
	{
		bg.sprite = regularBG;
	}
}
