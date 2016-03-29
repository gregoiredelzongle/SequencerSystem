using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using Headache.Sequencer;
using NodeEditorFramework;

namespace Headache.Sequencer
{
	[System.Serializable]
	public abstract class SequencerTask : ScriptableObject
	{

		public string actorName;
		public abstract string GetID();
		public abstract void DisplayArgsGUI(Node node,out float height);
		public abstract SequencerTask Create();
		public abstract void Init();
		public virtual void OnRemove(Node node){}

		public virtual List<ScriptableObject> GetScriptableObjects(){ return null;}
		public virtual void CopyScriptableObjects (System.Func<ScriptableObject, ScriptableObject> replaceSerializableObject) {}

		public abstract System.Type GetActorType();

	}
}
