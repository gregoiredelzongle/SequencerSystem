using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Headache.Sequencer;

namespace Headache.Sequencer
{
    public static class TaskTypes
    {
        public static List<SequencerTask> tasks;

        public static void FetchTasks()
        {
            tasks = new List<SequencerTask>();

            List<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where((Assembly assembly) => assembly.FullName.Contains("Assembly")).ToList();
            if (!scriptAssemblies.Contains(Assembly.GetExecutingAssembly()))
                scriptAssemblies.Add(Assembly.GetExecutingAssembly());
            foreach (Assembly assembly in scriptAssemblies)
            {
                foreach (Type type in assembly.GetTypes().Where(T => T.IsClass && !T.IsAbstract && T.IsSubclassOf(typeof(SequencerTask))))
                {
                        SequencerTask task = ScriptableObject.CreateInstance(type.Name) as SequencerTask; // Create a 'raw' instance (not setup using the appropriate Create function)
                        tasks.Add(task);
                }
            }
        }

        public static string[] TasksID()
        {
            if (tasks == null)
                FetchTasks();

            List<string> tasksName = new List<string>();
            foreach(SequencerTask task in tasks)
            {
                tasksName.Add(task.GetID());
            }
            return tasksName.ToArray();
        }

		public static SequencerTask getDefaultTask (string taskID)
		{
			if (tasks == null)
				FetchTasks();

			return tasks.Single<SequencerTask> ((SequencerTask task) => task.GetID() == taskID);
		}

		/// <summary>
		/// Returns the default node from the node type. 
		/// The default node is a dummy used to create other nodes (Due to various limitations creation has to be performed on Node instances)
		/// </summary>
		public static T getDefaultTask<T> () where T : SequencerTask
		{
			if (tasks == null)
				FetchTasks();
			
			return tasks.Single<SequencerTask> ((SequencerTask task) => tasks.GetType () == typeof (T)) as T;
		}

    }
}
