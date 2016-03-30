using UnityEngine;
using System.Collections;
using Headache.Sequencer;


namespace Headache.Sequencer
{
    /*
    Use this component with a Collider2D with "Trigger" enabled, and connect the Sequencer.
    */

    public class SequencerTrigger : MonoBehaviour
    {
        public enum SequencerRunOptions { OnActorEnter, OnActorExit, OnActorEnterAndPressActionButton }
        public enum SequencerCancelOptions { OnActorPressCancelButton, OnActorExit }

        public SequencerPlayer sequencer;

        public SequencerRunOptions sequencerRunOptions = SequencerRunOptions.OnActorEnter;
        public SequencerCancelOptions sequencerCancelOptions = SequencerCancelOptions.OnActorPressCancelButton;
        

        // Called by SequencerInteractor when a actor enter the trigger zone
        public void OnActorEntered(SequencerInteractor actor)
        {
            //Debug.Log(actor.name + " entered SequencerTrigger " + name);

            if (sequencerRunOptions == SequencerRunOptions.OnActorEnter && sequencer != null)
            {
                StartSequencer(actor);
            }

        }

        // Called by SequencerInteractor when a actor exit the trigger zone
        public void OnActorExited(SequencerInteractor actor)
        {
            //Debug.Log(actor.name + " exited SequencerTrigger " + name);

            if (sequencerRunOptions == SequencerRunOptions.OnActorExit && sequencer != null)
            {
                StartSequencer(actor);
            }
            if (sequencerCancelOptions == SequencerCancelOptions.OnActorExit && sequencer != null)
            {
                sequencer.RequestCancel(actor);
            }


        }

        // Called by SequencerInteractor when a actor enter the trigger zone then press the action button
		public void OnActorEnteredAndPressedActionButton(SequencerInteractor actor)
        {
            //Debug.Log(actor.name + " entered and pressed button in SequencerTrigger " + name);

            if (sequencerRunOptions == SequencerRunOptions.OnActorEnterAndPressActionButton && sequencer != null)
            {
                StartSequencer(actor);
            }
        }

        // Start the sequencer
        void StartSequencer(SequencerInteractor actor)
        {
            if (sequencerCancelOptions == SequencerCancelOptions.OnActorPressCancelButton)
            {
                actor.FreezeCharacter(true);
                actor.cancelButtonPressed += sequencer.RequestCancel;

            }
            sequencer.RequestRun(actor);
        }




    }
}
