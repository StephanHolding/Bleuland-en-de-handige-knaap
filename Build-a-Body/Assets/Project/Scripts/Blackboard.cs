using System.Collections.Generic;

public static class Blackboard
{

    public static Dictionary<string, object> blackboard = new Dictionary<string, object>();

    public static bool HasKey(string key)
    {
        return blackboard.ContainsKey(key);
    }

    public static void Write(string key, object value)
    {
        if (blackboard.ContainsKey(key))
        {
            blackboard[key] = value;
        }
        else
        {
            blackboard.Add(key, value);
        }
    }

    public static T Read<T>(string key)
    {
        if (blackboard.ContainsKey(key))
        {
            return (T)blackboard[key];
        }

        return default(T);
    }

    public static void RemoveKey(string key)
    {
        if (HasKey(key))
        {
            blackboard.Remove(key);
        }
    }
}

public static class BlackboardKeys
{
    public const string LAST_FINISHED_MINIGAME = "lfm";
    public const string LOCKED_ORGANS = "lo";
}
