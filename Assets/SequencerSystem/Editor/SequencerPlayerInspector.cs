using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

using NodeEditorFramework;
using Headache.Sequencer;


[CustomEditor(typeof(SequencerPlayer))]
public class SequencerPlayerInspector : Editor {


	private SequencerPlayer player;
	//SequencerStatus status;

	public void OnEnable()
	{
		player = target as SequencerPlayer;
	}

	public override void OnInspectorGUI()
	{

		EditorGUI.BeginChangeCheck ();
		player.canvas = EditorGUILayout.ObjectField (player.canvas, typeof(NodeCanvas), true) as NodeCanvas;

		if(EditorGUI.EndChangeCheck())
		{
			if (player.canvas == null)
				player.sequencer = null;
			else {
				player.sequencer = Sequencer.Create (player.canvas);
			}

		}


		if (player.canvas == null || player.sequencer == null) {
			EditorGUILayout.LabelField ("(Drop a canvas here)");
			return;
		}

		if(GUILayout.Button("Refresh Canvas"))
			player.sequencer.Refresh (player.canvas);

		player.sequencer.InspectorGUI ();


	}
}
