using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[Node(false, "Sequencer/StartSequencer")]
public class StartSequencerNode : Node
{
    public const string ID = "startSequencer";
    public override string GetID { get { return ID; } }

	public override bool AllowDeleteRequest {
		get {
			return false;
		}
	}

	public override bool AllowDuplicateRequest {
		get {
			return false;
		}
	}

	public override bool AllowCreateInContextMenu {
		get {
			return false;
		}
	}

    public override Node Create(Vector2 pos)
    {
        StartSequencerNode node = CreateInstance<StartSequencerNode>();

        node.rect = new Rect(pos.x, pos.y, 150, 60);
        node.name = "Sequencer Start";

        NodeOutput.Create(node,"START", "Float");

        return node;
    }

    protected internal override void NodeGUI()
    {
        GUILayout.Label(" ");
        OutputKnob(0);
    }

    public override bool Calculate()
    {
        if (!allInputsReady())
            return false;
        //Outputs[0].SetValue<float>(Inputs[0].GetValue<float>() * 5);
        return true;
    }

	protected internal override void CopyScriptableObjects (System.Func<ScriptableObject, ScriptableObject> replaceSerializableObject) 
	{
		
	}
}

