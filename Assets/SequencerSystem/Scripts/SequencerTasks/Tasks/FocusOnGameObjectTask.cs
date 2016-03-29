using UnityEngine;
using System.Collections;
using Headache.Sequencer;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

using System;

namespace Headache.Sequencer
{
    public class FocusOnGameObjectTask : SequencerTask
    {
        private const string id = "Focus On GameObject";

        public override string GetID()
        {
            return id;
        }

        public float lerpAmount;

        public FocusOnGameObjectTask(float lerpAmount)
        {
            this.lerpAmount = lerpAmount;
        }

		public override void DisplayArgsGUI(Node node,out float height)
        {
            //dialogue = GUILayout.TextArea(dialogue);
            height = 0;
        }

        public override void Init()
        {
            lerpAmount = 0f;
        }

		public override SequencerTask Create()
		{
			FocusOnGameObjectTask task = CreateInstance<FocusOnGameObjectTask> ();
			task.Init ();
			return task as SequencerTask;
		}

		public override System.Type GetActorType(){return typeof(FocusOnGameObject);}

    }
}

