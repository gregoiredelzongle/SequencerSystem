using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

using System.Linq;

using Headache.Sequencer;
using NodeEditorFramework;

namespace Headache.Sequencer{

	/// <summary>
	/// Hold data and deal with sequencer interactions
	/// </summary>
	[System.Serializable]
	public class Sequencer : ScriptableObject {

		#region Public Variables

		/// <summary>
		/// The canvas which contains all the nodes and connections drawn in the node editor
		/// </summary>
		public NodeCanvas canvas;

		/// <summary>
		/// return the first node linked with "SequenceStart" in the node editor or return null if something's wrong
		/// </summary>

		public SequenceNode Start {
			get {
				try{
				NodeOutput output = canvas.start.nodeKnobs [0] as NodeOutput;
				return output.connections [0].body as SequenceNode;
				}
				catch {
					return null;
				}
			}
		}

		#endregion

		#region Actor Dictionary
		/// <summary>
		/// Dictionary which contains connections between tasks and actors
		/// </summary>
		public Dictionary<SequencerTask,SequencerActor> Actors
		{
			get{
				if (_actors == null) {
					//Retrieve Dictionary data
					_actors = new Dictionary<SequencerTask, SequencerActor>();
					for (int i = 0; i < actorsKey.Count; i++) {
						_actors.Add (actorsKey [i], actorsValue [i]);
					}
				}
				return _actors;
			}
			set{
				//Store keys in a list
				actorsKey = value.Keys.ToList();
				actorsValue = value.Values.ToList ();
				_actors = value;

			}
		}
		// Non serialized Dictionary, don't use it directly, use Actors instead
		private Dictionary<SequencerTask,SequencerActor> _actors;

		// Store values contained in the dictionary because why the fuck not (plus serialization)
		[SerializeField]
		private List<SequencerTask> actorsKey;
		[SerializeField]
		private List<SequencerActor> actorsValue;

		#endregion 

		#region Private Variables

		/// <summary>
		/// Current node read by the sequencer
		/// </summary>
		private SequenceNode currentNode;
		private NodeOutput currentOutput;

		#endregion


		#region Callbacks

		public delegate void SequencerCallbacks(Sequencer sq);
		public SequencerCallbacks sequencerFinished;

		#endregion

		#region Create

		/// <summary>
		/// Create a new sequencer, stores the canvas and initialize itself
		/// </summary>
		public static Sequencer Create(NodeCanvas canvas)
		{
			// Create a sequencer instance
			Sequencer sequencer = CreateInstance<Sequencer> ();

			// Store canvas data
			sequencer.canvas = canvas;

			// Retrieve every task into the canvas that need to be linked
			Dictionary<SequencerTask,SequencerActor> dictionary = GetActorsFromCanvas(sequencer.canvas);

			// Save the dictionary 
			sequencer.Actors = dictionary;
			return sequencer;
		}

		#endregion

		#region Sequencer Controls

		/// <summary>
		/// Run the Sequencer and optionally restart it if it's already running.
		/// </summary>

		public void Run()
		{
			currentNode = Start;
			RunNodeTasks (currentNode);
		}

		/// <summary>
		/// Cancel  every task in the sequencer current node
		/// </summary>

		public void Cancel(bool reset = true)
		{
			foreach (SequencerTask task in currentNode.tasks) {
				Actors [task].Cancel ();
			}
				
			if(reset)
				currentNode = Start;
			
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Runs the node tasks.
		/// </summary>
		/// <param name="node">Node.</param>

		private void RunNodeTasks(SequenceNode node)
		{
			currentOutput = null;

			foreach (SequencerTask task in node.tasks) {

				// Check if actor associated with task is not null
				if (Actors [task] == null) {
					Debug.LogError ("Actor associated with " + task.actorName + " missing");
					return;
				}
				// Callback for when the task is finished
				Actors [task].onTaskFinished += OnTaskFinished;

				// Run the task
				Actors [task].Run (task);
			}
		}

		private static Dictionary<SequencerTask,SequencerActor> GetActorsFromCanvas(NodeCanvas canvas, Dictionary<SequencerTask,SequencerActor> baseActors = null)
		{
			Dictionary<SequencerTask,SequencerActor> dictionary = new Dictionary<SequencerTask, SequencerActor> ();

			foreach(Node node in canvas.nodes)
			{

				// Check if the node is a Sequence Node or a subclass of it
				if (node.GetType().IsSubclassOf(typeof(SequenceNode)) || node.GetType() == typeof(SequenceNode)) {

					// Recast the node as a sequenceNode to be able to retrieve tasks
					SequenceNode sqNode = node as SequenceNode;

					// Iterate and store tasks contained in the sequence Node 
					foreach (SequencerTask task in sqNode.tasks) {

						if (baseActors != null && baseActors.ContainsKey (task)) {
							dictionary.Add (task, baseActors [task]);
						}
						else
							dictionary.Add (task, null);
					}
				}
			}
			return dictionary;
		}

		#endregion

		#region Sequencer Events

		/// <summary>
		/// Called when user input is registered
		/// </summary>
		/// <param name="id">Button Identifier.</param>

		public void OnButtonPressed(int id)
		{

			// Pass the event to every task currently running
			foreach (SequencerTask task in currentNode.tasks) {
				Actors [task].ButtonPressed (id);
			}
		}

		/// <summary>
		/// Called when one actor has finished a task.
		/// </summary>
		/// <param name="actor">The actor .</param>
		private void OnTaskFinished(SequencerActor actor, NodeOutput output = null)
		{
			if(output != null)
				currentOutput = output;

			actor.onTaskFinished -= OnTaskFinished;

			// Check if every task is finished
			if (actor.onTaskFinished == null)
				OnNodeTasksFinished (currentNode);

		}

		/// <summary>
		/// Called when every actors has finished their task
		/// </summary>
		/// <param name="node">Node.</param>
		private void OnNodeTasksFinished(SequenceNode node)
		{
			// Check every output 
			NodeOutput[] outputs = node.GetOutputs();

			NodeOutput output = currentOutput != null ? currentOutput :outputs [0];

			// Check if output is connected to something
			if (output.connections.Count > 0) {
				
				//TODO check if connected input node is a sequence node
				currentNode = output.connections [0].body as SequenceNode;
				RunNodeTasks (currentNode);

			} else {
				// Raises Sequencer Finished event if no outputs seems to be connected
				if(sequencerFinished != null)
					sequencerFinished (this);
				Debug.Log ("Sequencer Finished !!");
			}

		}

		#endregion

		#if UNITY_EDITOR


		// Used to reference actors without missing references (not working)
		public void Refresh(NodeCanvas canvas)
		{
			this.canvas = canvas;
			Actors = GetActorsFromCanvas(canvas,Actors);
		}



		public void InspectorGUI()
		{
			EditorGUILayout.LabelField ("Actors objects");

			for (int i = 0; i < Actors.Keys.Count; i++) {
				SequencerTask task = actorsKey [i];

				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.PrefixLabel (task.actorName);

				EditorGUI.BeginChangeCheck ();
				Actors [task] = EditorGUILayout.ObjectField (
					Actors [task],
					task.GetActorType (),
					true
				) as SequencerActor;
				if (EditorGUI.EndChangeCheck ()) {
					EditorUtility.SetDirty (this);
					Actors = _actors;
				}
				EditorGUILayout.EndHorizontal ();

			}
		}
		#endif

	}
}
