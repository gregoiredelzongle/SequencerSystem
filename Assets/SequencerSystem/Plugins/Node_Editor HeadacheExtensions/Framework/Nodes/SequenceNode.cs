using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Headache.Sequencer;

[System.Serializable]
[Node(false, "Sequencer/SequenceNode")]
public class SequenceNode : Node {

    public const string ID = "SequenceNode";
    public override string GetID { get { return ID; } }

    private int actionSelection = -1;

    public List<SequencerTask> actions;

    public override Node Create(Vector2 pos)
    {
        SequenceNode node = CreateInstance<SequenceNode>();

        node.rect = new Rect(pos.x, pos.y, 300, 150);
        node.name = "Sequence";

		node.actions = new List<SequencerTask>();
        node.CreateInput("TRIGGER", "Float");
        node.CreateOutput("FINISHED", "Float");

        return node;
    }

    protected internal override void NodeGUI()
    {
        rect.height = 75;


        for (int i = 0; i < actions.Count; i++)
        {
            if(actions[i] != null)
                 DisplayTaskGUI(actions[i]);
        }
        rect.height += 20;
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        actionSelection = RTEditorGUI.Popup(actionSelection, TaskTypes.TaskNames());
        if (GUILayout.Button("Add Action...") && actionSelection != -1)
        {
            System.Type argsType = SequencerProperties.GetSequenceActionArgsTypes()[actionSelection];
			SequencerTask args = ScriptableObject.CreateInstance(argsType.Name) as SequencerTask;
			//NodeEditorSaveManager.AddSubAsset (args, this);

            args.Init();
            actions.Add(args);
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        Inputs[0].DisplayLayout();

        GUILayout.EndVertical();
        GUILayout.BeginVertical();

        Outputs[0].DisplayLayout();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

    }

	private void DisplayTaskGUI(SequencerTask args)
    {
        GUILayout.BeginVertical(NodeEditorGUI.nodeBox);

        GUILayout.BeginHorizontal();
        GUILayout.Label(args.GetID());
        if (GUILayout.Button("x",GUILayout.Width(20)))
            DeleteAction(args);
        GUILayout.EndHorizontal();
        float height;
        args.DisplayArgsGUI(this,out height);
        rect.height += height;
        GUILayout.EndVertical();

    }

	private void DeleteAction(SequencerTask args)
    {
		args.OnRemove (this);
        actions.Remove(args);
    }

    private NodeOutput CreateOutput()
    {
        CreateOutput(" ", "Float");
        return Outputs[Outputs.Count-1];
    }

    private void DeleteOutput(NodeOutput output)
    {
        nodeKnobs.Remove(output);
    }

    private void DisplayOutput(NodeOutput output)
    {
       // output.DisplayLayout();
    }

    public override bool Calculate()
    {
        if (!allInputsReady())
            return false;
        return true;
    }

    protected internal override ScriptableObject[] GetScriptableObjects() {
        List<ScriptableObject> objToSerialize = new List<ScriptableObject>();
		foreach (SequencerTask action in actions)
        {
            objToSerialize.Add(action);

            if(action.GetScriptableObjects() != null)
                objToSerialize.AddRange(action.GetScriptableObjects());
        }
		return objToSerialize.ToArray();
    }

	protected internal override void CopyScriptableObjects (System.Func<ScriptableObject, ScriptableObject> replaceSerializableObject) 
	{
		for(int i = 0; i < actions.Count;i++)
		{
			actions[i] = replaceSerializableObject.Invoke (actions[i]) as SequencerTask;
			actions[i].CopyScriptableObjects (replaceSerializableObject);
		}
	}
}
