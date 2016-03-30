using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Headache.Sequencer;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;

using System;


namespace Headache.Sequencer
{
   

    [System.Serializable]
    public class PlayDialogueTask : SequencerTask
    {
        private const string id = "Play Dialogue";

        public override string GetID()
        {
            return id;
        }
        public string dialogue;

        

        public List<Reply> replies;

        public override void Init()
        {
            replies = new List<Reply>();
            this.actorName = "";
            this.dialogue = "";
        }

		public override SequencerTask Create()
		{
			PlayDialogueTask task = CreateInstance<PlayDialogueTask> ();
			task.Init ();
			return task as SequencerTask;
		}

        public override void DisplayArgsGUI(Node node, out float height)
        {
            float h = 20;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Actor Name : ");
            actorName = GUILayout.TextField(actorName);
            GUILayout.EndHorizontal();

            h += 50;
			dialogue = GUILayout.TextArea(dialogue,EditorStyles.textArea,GUILayout.Height(50));
            
            for(int i = 0; i < replies.Count;i++)
            {
				
                h += 20;
                GUILayout.BeginHorizontal();
				if (replies [i] != null) {
					
					replies [i].text = GUILayout.TextField (replies [i].text,GUILayout.Width(250));
					replies [i].output.DisplayLayout(new GUIContent(""));

					if (GUILayout.Button ("x", GUILayout.Width (20))) {
						RemoveReply (node, replies [i]);
					}
				} else {
					GUILayout.Label("Reply NULL, Check Serialization");
				}
                GUILayout.EndHorizontal();
                    
            }
            h += 50;
            if (GUILayout.Button("Add Reply"))
            {
				Reply reply = Reply.Create (node);
				replies.Add (reply);
            } 
            height = h;

        }

		private void RemoveReply(Node node,Reply reply)
		{
			node.nodeKnobs.Remove (reply.output);
			replies.Remove (reply);
		}

		public override void OnRemove(Node node)
		{
			foreach (Reply reply in replies) {
				node.nodeKnobs.Remove (reply.output);
			}
		}

        public override List<ScriptableObject> GetScriptableObjects()
        {
            List<ScriptableObject> objectsToSerialize = new List<ScriptableObject>();
            foreach(Reply reply in replies)
            {
                objectsToSerialize.Add(reply);
            }
            return objectsToSerialize;
        }

		public override void CopyScriptableObjects (Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
		{
			for (int i = 0; i < replies.Count; i++)
			{
				replies[i] = replaceSerializableObject.Invoke(replies[i]) as Reply;
				replies[i].output = replaceSerializableObject.Invoke(replies[i].output) as NodeOutput;
			}
		}

		public override System.Type GetActorType(){return typeof(PlayDialogueActor);}
    }

	[System.Serializable]
	public class Reply : ScriptableObject
	{
		public string text;
		public NodeOutput output;

		public void Init(string text, NodeOutput output)
		{
			this.text = text;
			this.output = output;
		}

		public static Reply Create(Node node)
		{
			node.CreateOutput ("", "Float");

			NodeOutput output = node.nodeKnobs [node.nodeKnobs.Count - 1] as NodeOutput;
			Reply reply = CreateInstance<Reply>();
			reply.Init(" ", output);
			return reply;
		}




	}
}
