using UnityEngine;
using System;

[Serializable]
public class EmotionalValue
{
    [Range(0f,1f)] public float anger;
    [Range(0f,1f)] public float suspicion;
    [Range(0f,1f)] public float happiness;
    [Range(0f,1f)] public float regret;
    

    public string GetDominantEmotion()
    {
        return string.Empty;
    }
    
    public static EmotionalValue operator + (EmotionalValue a, EmotionalValue b)
    {
        return new EmotionalValue
        {
            anger     = a.anger     + b.anger,
            suspicion = a.suspicion + b.suspicion,
            happiness = a.happiness + b.happiness,
            regret    = a.regret    + b.regret
        };
    }

    public static EmotionalValue operator /(EmotionalValue a, int b)
    {
        return new EmotionalValue
        {
            anger     = a.anger     / b,
            suspicion = a.suspicion / b,
            happiness = a.happiness / b,
            regret    = a.regret    / b,
        };
    }

    public void Empty()
    {
        anger = 0f;
        suspicion = 0f;
        happiness = 0f;
        regret = 0f;
    }
}