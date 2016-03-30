using UnityEngine;
using System.Collections;

using Headache.Sequencer;

namespace Headache.Sequencer{

	public class PlayDialogueActor : SequencerActor<PlayDialogueTask> {

		public SpeechBubble speechBubble;
		public float speed = 1f;

		private int replySelect = -1;

		delegate void SequencerActorCallBack(PlayDialogueTask task);

		SequencerActorCallBack onInteractButtonPress;
		SequencerActorCallBack onUpButtonPress;
		SequencerActorCallBack onDownButtonPress;



        public override void Run(PlayDialogueTask task)
        {
			replySelect = task.replies.Count > 0 ? 0 : -1;
			isRunning = true;
			this.task = task;
			onInteractButtonPress = DisplayDialogue;

			StartCoroutine ("MakeDialogueScroll",task);
        }

        public override void Cancel()
        {
			onUpButtonPress = null;
			onDownButtonPress = null;
			onInteractButtonPress = null;

			StopCoroutine ("MakeDialogueScroll");
			speechBubble.DestroyReplies ();
			speechBubble.gameObject.SetActive (false);
			isRunning = false;


        }

        public override void ButtonPressed(int id)
        {
			if (task == null)
				return;

			switch (id) {
			case 0:
				if (onInteractButtonPress != null)
					onInteractButtonPress (task);
				break;
			case 1:
				if (onUpButtonPress != null)
					onUpButtonPress (task);
				break;
			case 2:
				if (onDownButtonPress != null)
					onDownButtonPress (task);
				break;
			}
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

			foreach (Reply reply in task.replies) {
				speechBubble.CreateReply (reply.text);
			}

			if (task.replies.Count > 0) {
				speechBubble.replies[replySelect].Select ();
				onUpButtonPress = (PlayDialogueTask args) => {SwitchReply (args,-1);};
				onDownButtonPress = (PlayDialogueTask args) => {SwitchReply (args,1);};
			}

			onInteractButtonPress = EndDialogue;
		}

		void SwitchReply(PlayDialogueTask task,int dir)
		{
			int max = task.replies.Count;

			speechBubble.replies[replySelect].Unselect ();
			replySelect = Mathf.RoundToInt(Mathf.Repeat(replySelect + dir,max));
			speechBubble.replies[replySelect].Select ();
		}
			

		void EndDialogue(PlayDialogueTask task)
		{
			Cancel ();

			if (onTaskFinished != null) {
				if(replySelect == -1)
					onTaskFinished (this);
				else
					onTaskFinished (this,task.replies[replySelect].output);
			}
		}


    }
}
