using UnityEngine;
using System.Collections;
using Headache.Sequencer;


namespace Headache.Sequencer
{
    /* 
    Drag this component to the player character in order to make it interact with any sequence
    */


    public class SequencerInteractor : MonoBehaviour
    {
        public CharacterControls characterControls;


        public delegate void InteractorEvents(SequencerInteractor actor);
        public InteractorEvents actionButtonPressed;
        public InteractorEvents cancelButtonPressed;


        void OnTriggerEnter2D(Collider2D other)
        {
            SequencerTrigger sqTrigger = other.GetComponent<SequencerTrigger>();
            if(sqTrigger != null)
            {
                sqTrigger.OnActorEntered(this);
                actionButtonPressed += sqTrigger.OnActorEnteredAndPressedActionButton;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            SequencerTrigger sqTrigger = other.GetComponent<SequencerTrigger>();
            if (sqTrigger != null)
            {
                sqTrigger.OnActorExited(this);
                actionButtonPressed -= sqTrigger.OnActorEnteredAndPressedActionButton;
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Action") && actionButtonPressed != null)
                actionButtonPressed(this);

            if (Input.GetButtonDown("Cancel") && cancelButtonPressed != null)
                cancelButtonPressed(this);

        }

        public void FreezeCharacter(bool state)
        {
            characterControls.enabled = !state;
        }

    }
}
