using UnityEngine;
using System.Collections;

using Headache.Sequencer;

namespace Headache.Sequencer{

	public class PlayDialogueActor : SequencerActor<PlayDialogueTask> {

		public SpeechBubble speechBubble;
		public float speed = 1f;

		delegate void SequencerActorCallBack(PlayDialogueTask task);
		SequencerActorCallBack onInteractButtonPress;

        public override void Run(PlayDialogueTask task)
        {
			isRunning = true;
			this.task = task;
			onInteractButtonPress = DisplayDialogue;

			StartCoroutine ("MakeDialogueScroll",task);
        }

        public override void Cancel()
        {
			StopCoroutine ("MakeDialogueScroll");
			speechBubble.gameObject.SetActive (false);
			isRunning = false;

			if(onTaskFinished != null)
				onTaskFinished(this);
        }

        public override void ButtonPressed(int id)
        {
			if (onInteractButtonPress != null && task != null && id == 0 )
				onInteractButtonPress (task);
        }

		IEnumerator MakeDialogueScroll(PlayDialogueTask task)
		{
			speechBubble.gameObject.SetActive(true);
			speechBubble.Name = name;

			string displayingText = speechBubble.Dialogue = "";
			float intervalTime = speed/10;
			int i = 0;

			do 
			{
				for(float t = 0; t < intervalTime; t += Time.deltaTime)
				{
					yield return 0;
				}
				displayingText += task.dialogue[i++];
				speechBubble.Dialogue = displayingText;

			} while(displayingText != task.dialogue);

			DisplayDialogue (task);

		}

		void DisplayDialogue(PlayDialogueTask task)
		{
			StopCoroutine ("MakeDialogueScroll");
			speechBubble.Dialogue = task.dialogue;

			onInteractButtonPress = EndDialogue;
		}

		void EndDialogue(PlayDialogueTask task)
		{
			Cancel ();
		}


    }
}
