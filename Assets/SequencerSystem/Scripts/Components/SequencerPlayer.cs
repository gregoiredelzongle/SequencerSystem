using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Headache.Sequencer;
using NodeEditorFramework;

using System.Linq;

namespace Headache.Sequencer
{
    public class SequencerPlayer : MonoBehaviour
    {
        public bool isRunning { get; private set; }

        public NodeCanvas canvas;
		public Sequencer sequencer;

        public void LoadSequencer()
        {
			if(canvas != null)
				sequencer = Sequencer.Create (canvas);
        }

        public void Run()
        {

            //Debug.Log("Sequencer running");
            isRunning = true;
			sequencer.Restart ();
        }

        public void OnActionButtonPressed(SequencerInteractor requester)
        {
 
			sequencer.OnButtonPressed (0);
            //Debug.Log("Action Button Pressed");
        }

        public void Cancel()
        {
            //Debug.Log("Sequencer stopped");
            isRunning = false;
        }

        public void RequestRun(SequencerInteractor requester)
        {
			sequencer.sequencerFinished += (Sequencer sq) => { requester.FreezeCharacter(false);};
            requester.actionButtonPressed = OnActionButtonPressed;
            Run();
        }

        public void RequestCancel(SequencerInteractor requester)
        {
            requester.cancelButtonPressed -= this.RequestCancel;
            requester.actionButtonPressed -= this.OnActionButtonPressed;

            requester.FreezeCharacter(false);
            Cancel();

        }


    }
}
