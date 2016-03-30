using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Headache.Sequencer;
using System;

namespace Headache.Sequencer {
    public static class SequencerProperties {

        public static Type[] GetSequenceActionArgsTypes()
        {
            List<Type> types = new List<Type>();
            List<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where((Assembly assembly) => assembly.FullName.Contains("Assembly")).ToList();
            if (!scriptAssemblies.Contains(Assembly.GetExecutingAssembly()))
                scriptAssemblies.Add(Assembly.GetExecutingAssembly());
            foreach (Assembly assembly in scriptAssemblies)
            {
				foreach (Type type in assembly.GetTypes().Where(T => T.IsSubclassOf(typeof(SequencerTask))))
                {
                    types.Add(type);
                }
            }
            return types.ToArray();

        }

		public static SequencerTask CreateArgs(Type type)
        {
            
			SequencerTask args = ScriptableObject.CreateInstance(type.FullName) as SequencerTask;
            return args;
        }
    }
}
