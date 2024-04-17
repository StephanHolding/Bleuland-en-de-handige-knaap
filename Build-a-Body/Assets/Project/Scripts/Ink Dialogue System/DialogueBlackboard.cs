using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Blackboard
{
	public static class DialogueBlackboard
	{

		private static Dictionary<string, object> data = new Dictionary<string, object>();

		public static void SetVariable(string key, object value)
		{
			if (data.ContainsKey(key))
			{
				data[key] = value;
			}
			else
			{
				data.Add(key, value);
			}
		}

		public static T GetVariable<T>(string key)
		{
			return (T)data[key];
		}

		public static void Reset()
		{
			data.Clear();
		}
		public static bool HasKey(string key)
		{
			return data.ContainsKey(key);
		}
	}

}

