using UnityEngine;
using System.Collections;
using Headache.Sequencer;


namespace Headache.Sequencer
{
    /* 
    Drag this component to the player character in order to make it interact with any sequence
    */

	public enum InteractorButtons
	{
		Up,
		Down,
		Action,
		Cancel
	}

    public class SequencerInteractor : MonoBehaviour
    {
        public CharacterControls characterControls;


		public delegate void InteractorEvents(SequencerInteractor actor);
        public InteractorEvents actionButtonPressed;
		public InteractorEvents cancelButtonPressed;
		public InteractorEvents upButtonPressed;
		public InteractorEvents downButtonPressed;





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
			CheckInputs ();
        }

		void CheckInputs()
		{
			if (Input.GetButtonDown("Action") && actionButtonPressed != null)
				actionButtonPressed(this);

			if (Input.GetButtonDown("Cancel") && cancelButtonPressed != null)
				cancelButtonPressed(this);
			
			if (Input.GetKeyDown(KeyCode.UpArrow)  && upButtonPressed != null)
				upButtonPressed(this);

			if (Input.GetKeyDown(KeyCode.DownArrow) && downButtonPressed != null)
				downButtonPressed(this);

		}

        public void FreezeCharacter(bool state)
        {
            characterControls.enabled = !state;
        }

    }
}
