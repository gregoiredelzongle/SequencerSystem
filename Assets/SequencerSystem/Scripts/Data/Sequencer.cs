using UnityEngine;
using System.Collections.Generic;

using System.Linq;

using Headache.Sequencer;
using NodeEditorFramework;

namespace Headache.Sequencer{

	public class Sequencer : ScriptableObject {

		public NodeCanvas canvas;
		public SequenceNode start;
		public Dictionary<SequencerTask,SequencerActor> links;

		[SerializeField]
		public List<SequencerTask> linksKey;
		[SerializeField]
		public List<SequencerActor> linksValue;

		private SequenceNode currentNode;

		public delegate void SequencerCallbacks(Sequencer sq);
		public SequencerCallbacks sequencerFinished;


		public static Sequencer Create(NodeCanvas canvas)
		{
			Sequencer sequencer = CreateInstance<Sequencer> ();

			sequencer.canvas = canvas;

			Dictionary<SequencerTask,SequencerActor> dictionary = new Dictionary<SequencerTask,SequencerActor> ();

			foreach(Node node in canvas.nodes)
			{
				if (node.GetID == "startSequencer" && sequencer.start == null) {

					NodeOutput output = node.nodeKnobs [0] as NodeOutput;

					if (output.connections.Count == 0)
						Debug.LogError ("StartSequence node not connected");
					else {
						sequencer.start = output.connections [0].body as SequenceNode;
					}
				}
				if (node.GetID == "SequenceNode") {
					SequenceNode sqNode = node as SequenceNode;
					foreach (SequencerTask args in sqNode.actions) {
						dictionary.Add (args, null);
					}
				}
			}

			sequencer.InitDictionary (dictionary);

			//Debug.Log ("Sequencer successfully loaded : " + sequencer.links.Count + " actions loaded");

			return sequencer;
		}

		public void InitDictionary(Dictionary<SequencerTask,SequencerActor> dictionary)
		{
			linksKey = dictionary.Keys.ToList ();
			linksValue = dictionary.Values.ToList ();
			links = dictionary;
		}

		public void RefreshDictionary()
		{
			Dictionary<SequencerTask,SequencerActor> dictionary = new Dictionary<SequencerTask,SequencerActor> ();
			for (int i = 0; i < linksKey.Count; i++) {
				dictionary.Add (linksKey [i], linksValue [i]);
			}
			links = dictionary;
		}

		public void Restart()
		{
			RefreshDictionary ();
			currentNode = start;
			RunNodeTasks (currentNode);
		}


		public void RunNodeTasks(SequenceNode node)
		{
			foreach (SequencerTask args in node.actions) {
				if (links [args] == null)
					Debug.LogError ("Actor not found");

				links [args].onTaskFinished += OnTaskFinished;
				links [args].Run (args);
			}
		}

		public void OnButtonPressed(int id)
		{
			if (currentNode == null)
				return;

			foreach (SequencerTask args in currentNode.actions) {
				links [args].ButtonPressed (id);
			}
		}

		private void OnTaskFinished(SequencerActor actor)
		{
			actor.onTaskFinished -= OnTaskFinished;
			if (actor.onTaskFinished == null)
				OnNodeTasksFinished (currentNode);

		}

		private void OnNodeTasksFinished(SequenceNode node)
		{
			List<NodeOutput> output = new List<NodeOutput> ();
			foreach (NodeKnob nodeKnob in node.nodeKnobs) {
				if (nodeKnob is NodeOutput)
					output.Add ((NodeOutput)nodeKnob);
			}
				
			if (output.Count > 0 && output [0].connections.Count > 0) {
				currentNode = output [0].connections [0].body as SequenceNode;
				RunNodeTasks (currentNode);
			} else {
				if(sequencerFinished != null)
					sequencerFinished (this);
				Debug.Log ("Sequencer Finished !!");
			}
				
		}

	}
}
