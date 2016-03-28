using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

	public Text nameDisplay;
	public Text dialogueDisplay;

	public string Name { get { return nameDisplay.text; } set { nameDisplay.text = value; }}
	public string Dialogue { get { return dialogueDisplay.text; } set { dialogueDisplay.text = value; }}
}
