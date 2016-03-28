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

        public static string[] TaskNames()
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

    }
}
