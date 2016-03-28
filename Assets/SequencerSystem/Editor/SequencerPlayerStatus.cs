using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Headache.Sequencer;

public class SequencerStatus : ScriptableObject {

	//public delegate void SequencerStatusEvents();
	//public SequencerStatusEvents  

	public string text { get; private set; }

	public static SequencerStatus Create(Sequencer sequencer)
	{
		SequencerStatus status = CreateInstance<SequencerStatus> ();
		status.Refresh(sequencer);
		return status;
	}

	public void Refresh(Sequencer sequencer)
	{
		text = "( "+GetStatus(sequencer)+" )\n";
		text += "( Not running )\n";
		//this.sequencer = sequencer;
	}

	#if UNITY_EDITOR
	public void ShowGUI()
	{
		EditorGUILayout.HelpBox (text,MessageType.Info);
	}
	#endif

	private string GetStatus(Sequencer sequencer)
	{
		if (sequencer == null)
			return "Sequencer not loaded";
		else
			return sequencer.canvas.name;
	}


}
