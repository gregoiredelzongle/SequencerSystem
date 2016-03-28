using UnityEngine;
using System.Collections;

using Headache.Sequencer;

namespace Headache.Sequencer{

	public class PlayDialogueActor : SequencerActor<PlayDialogueTask> {

		public SpeechBubble speechBubble;

        public override void Run(PlayDialogueTask task)
        {
			this.task = task;
			speechBubble.gameObject.SetActive(true);
			speechBubble.Name = name;
			speechBubble.Dialogue = task.dialogue;

			//Debug.Log (this.actorName + " : " + this.task.dialogue);
            //Debug.Log(args.dialogue);
        }

        public override void Cancel()
        {

        }

        public override void ButtonPressed(int id)
        {
			speechBubble.gameObject.SetActive (false);
			if(onTaskFinished != null)
            	onTaskFinished(this);
        }
    }
}
