using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Headache.Sequencer;


[System.Serializable]
[Node(false, "Dialogue/Simple Dialogue")]
public class DialogueNode : SequenceNode {

	#region Node parameters

	const string ID = "DialogueNode";
	public override string GetID { get { return ID; } }

	#endregion

	#region Sequence Parameters

	protected override bool ShowAddActionGUIPanel {
		get {
			return false;
		}
	}

	protected override bool AllowActionsDelete {
		get {
			return false;
		}
	}
	#endregion

	#region Node Methods

	public override Node Create(Vector2 pos)
	{
		DialogueNode node = CreateInstance<DialogueNode>();

		node.rect = new Rect(pos.x, pos.y, 300, 150);
		node.name = "Dialogue Node";

		node.tasks = new List<SequencerTask>();

		node.AddTask ("Play Dialogue");

		node.CreateInput("TRIGGER", "Float");
		node.CreateOutput("FINISHED", "Float");
		return node;
	} 

	public override bool Calculate()
	{
		return base.Calculate ();
	}

	#endregion




}
