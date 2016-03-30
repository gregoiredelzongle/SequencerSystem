using UnityEngine;

using NodeEditorFramework;
using Headache.Sequencer;

namespace Headache.Sequencer
{

	/// <summary>
	/// Actor used to run a task contained into a Sequencer
	/// </summary>

	public abstract class SequencerActor : MonoBehaviour
	{

		/// <summary>
		/// Feed a task to the actor and run it
		/// </summary>
		public abstract void Run(SequencerTask task);

		/// <summary>
		/// Cancel any running task
		/// </summary>
		public abstract void Cancel();

		/// <summary>
		/// Method called when button pressed detected
		/// </summary>
		public abstract void ButtonPressed(int id);

		public delegate void SequenceActionEvents(SequencerActor action, NodeOutput output = null);

		/// <summary>
		/// Method called when the task is finished
		/// </summary>
		public SequenceActionEvents onTaskFinished;

	}

	/// <summary>
	/// Generic SequenerActor class 
	/// </summary>
	public abstract class SequencerActor<T> : SequencerActor where T : SequencerTask
	{

		[HideInInspector]
		public T task;

		protected bool isRunning = false;

		public override void Run(SequencerTask task)
		{
			Run (task as T);
		}

		public abstract void Run(T task);


	}
    
    

}
