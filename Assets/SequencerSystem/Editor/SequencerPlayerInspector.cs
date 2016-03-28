using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

using Headache.Sequencer;

[CustomEditor(typeof(SequencerPlayer))]
public class SequencerPlayerInspector : Editor {

	SequencerPlayer component;
	SequencerStatus status;

	public void OnEnable()
	{
		component = target as SequencerPlayer;

		if (component.sequencer != null) {
			component.sequencer.RefreshDictionary ();
			status = SequencerStatus.Create (component.sequencer);
		}
	}

	public override void OnInspectorGUI()
	{
		component = target as SequencerPlayer;

		if (status != null) {
			status.ShowGUI ();
		}

		DrawDefaultInspector ();

		if(GUILayout.Button("Load Sequencer"))
			component.LoadSequencer();



		if (component.sequencer == null)
			return;
		
		List<SequencerTask> argsList = component.sequencer.links.Keys.ToList();

		foreach (SequencerTask args in argsList) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel (args.actorName);

			EditorGUI.BeginChangeCheck ();

			component.sequencer.links [args] = (SequencerActor)EditorGUILayout.ObjectField (
				(SequencerActor)component.sequencer.links [args],
				args.GetActorType(),
				true
			);

			if (EditorGUI.EndChangeCheck ()) {
				component.sequencer.InitDictionary (component.sequencer.links);
				EditorUtility.SetDirty (component.sequencer);

			}

			EditorGUILayout.EndHorizontal ();

		}
	}
}
