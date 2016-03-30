using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeechBubble : MonoBehaviour {

	public Text nameDisplay;
	public Text dialogueDisplay;

	public string Name { get { return nameDisplay.text; } set { nameDisplay.text = value; }}
	public string Dialogue { get { return dialogueDisplay.text; } set { dialogueDisplay.text = value; }}

	public SpeechBubbleReply replyPrefab;

	public List<SpeechBubbleReply> replies = new List<SpeechBubbleReply>();

	public void CreateReply(string text)
	{
		SpeechBubbleReply reply = Instantiate<SpeechBubbleReply>(replyPrefab);
		reply.transform.SetParent (transform, false);
		reply.text = text;
		replies.Add (reply);

	}

	public void DestroyReplies()
	{
		foreach (SpeechBubbleReply reply in replies) {
			Destroy (reply.gameObject);
		}
		replies.Clear ();
	}

}
