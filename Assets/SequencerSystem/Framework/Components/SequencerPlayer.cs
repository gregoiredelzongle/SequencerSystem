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
		[HideInInspector]
		public Sequencer sequencer;


        public void Run()
        {

            //Debug.Log("Sequencer running");
            isRunning = true;
			sequencer.Run ();
        }

        void OnActionButtonPressed(SequencerInteractor requester)
        {
			sequencer.OnButtonPressed (0);
        }

		void OnUpButtonPressed(SequencerInteractor requester)
		{
			sequencer.OnButtonPressed (1);
		}

		void OnDownButtonPressed(SequencerInteractor requester)
		{
			sequencer.OnButtonPressed (2);
		}



        public void Cancel()
        {
			sequencer.Cancel ();
            //Debug.Log("Sequencer stopped");
            isRunning = false;
        }

        public void RequestRun(SequencerInteractor requester)
        {
			sequencer.sequencerFinished += (Sequencer sq) => { requester.FreezeCharacter(false);};
            requester.actionButtonPressed = OnActionButtonPressed;
			requester.upButtonPressed = OnUpButtonPressed;
			requester.downButtonPressed = OnDownButtonPressed;
            Run();
        }

        public void RequestCancel(SequencerInteractor requester)
        {
            requester.cancelButtonPressed -= this.RequestCancel;
            requester.actionButtonPressed -= this.OnActionButtonPressed;
			requester.upButtonPressed -= this.OnUpButtonPressed;
			requester.downButtonPressed -= this.OnDownButtonPressed;

			Cancel();

            requester.FreezeCharacter(false);

        }


    }
}
