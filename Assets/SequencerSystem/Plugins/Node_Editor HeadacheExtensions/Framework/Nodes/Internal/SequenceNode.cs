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
[Node(false, "Sequencer/SequenceNode")]
public class SequenceNode : Node {


	#region Node parameters

    const string ID = "SequenceNode";
    public override string GetID { get { return ID; } }

	#endregion

	#region SequenceNode Parameters


	public List<SequencerTask> tasks = new List<SequencerTask>();

    protected int taskSelection = -1;

	protected virtual bool ShowAddActionGUIPanel{get{ return true; }} 
	protected virtual bool AllowActionsDelete{get{ return true; }} 

	#endregion

	#region Node Methods

    public override Node Create(Vector2 pos)
    {
        SequenceNode node = CreateInstance<SequenceNode>();

        node.rect = new Rect(pos.x, pos.y, 300, 150);
        node.name = "Sequence";

		node.tasks = new List<SequencerTask>();
        node.CreateInput("TRIGGER", "Float");
        node.CreateOutput("FINISHED", "Float");

        return node;
    }

	public override bool Calculate()
	{
		if (!allInputsReady())
			return false;
		return true;
	}

	#endregion

	#region GUI

    protected internal override void NodeGUI()
    {
        rect.height = 75;

        for (int i = 0; i < tasks.Count; i++)
        {
            if(tasks[i] != null)
                 DisplayTaskGUI(tasks[i]);
        }

		// AddAction Panel

		if (ShowAddActionGUIPanel) {
			rect.height += 20;
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			taskSelection = RTEditorGUI.Popup (taskSelection, TaskTypes.TasksID ());
			if (GUILayout.Button ("Add Action...") && taskSelection != -1) {
				AddTask (TaskTypes.TasksID()[taskSelection]);
			}
			GUILayout.EndHorizontal ();
		}


		// Display NodeKnobs
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
        if (AllowActionsDelete && GUILayout.Button("x",GUILayout.Width(20)))
            DeleteAction(args);
        GUILayout.EndHorizontal();
        float height;
        args.DisplayArgsGUI(this,out height);
        rect.height += height;
        GUILayout.EndVertical();

    }

	#endregion 

	#region SequenceNode Methods

	public NodeOutput[] GetOutputs()
	{
		List<NodeOutput> outputs = new List<NodeOutput> ();

		foreach (NodeKnob knob in nodeKnobs) {
			if (knob.GetType () == typeof(NodeOutput))
				outputs.Add (knob as NodeOutput);
		}

		return outputs.ToArray ();
	}

	protected void AddTask(string taskID)
	{
		
		SequencerTask task = TaskTypes.getDefaultTask(taskID);
		task = task.Create ();
		task.Init();
		tasks.Add(task);
	}


	private void DeleteAction(SequencerTask args)
    {

		args.OnRemove (this);
        tasks.Remove(args);
    }
	#endregion

    /*private NodeOutput CreateOutput()
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
	*/



    #region Node Serialization

    protected internal override ScriptableObject[] GetScriptableObjects() {
        List<ScriptableObject> objToSerialize = new List<ScriptableObject>();
		foreach (SequencerTask action in tasks)
        {
            objToSerialize.Add(action);

            if(action.GetScriptableObjects() != null)
                objToSerialize.AddRange(action.GetScriptableObjects());
        }
		return objToSerialize.ToArray();
    }

	protected internal override void CopyScriptableObjects (System.Func<ScriptableObject, ScriptableObject> replaceSerializableObject) 
	{
		for(int i = 0; i < tasks.Count;i++)
		{
			tasks[i] = replaceSerializableObject.Invoke (tasks[i]) as SequencerTask;
			tasks[i].CopyScriptableObjects (replaceSerializableObject);
		}
	}

	#endregion
}
