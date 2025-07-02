using UnityEngine;
using System;

[Serializable]
public class EmotionalValue
{
    [Range(0f,1f)] public float anger;
    [Range(0f,1f)] public float suspicion;
    [Range(0f,1f)] public float happiness;
    [Range(0f,1f)] public float regret;
    

    public int GetDominantEnding()
    {
        // Find the max value among the four
        float maxValue = Mathf.Max(happiness, suspicion, regret, anger);

        if (Mathf.Approximately(maxValue, happiness))
            return 0; // happy
        else if (Mathf.Approximately(maxValue, suspicion))
            return 1; // suspicion
        else if (Mathf.Approximately(maxValue, regret) || Mathf.Approximately(maxValue, anger))
            return 2; // sad or anger

        // fallback, should not happen if values are valid
        return -1;    
    }
    
    
    
    public int GetDominantEmotion()
    {
        // Find the max value among the four
        float maxValue = Mathf.Max(happiness, regret, anger);

        if (Mathf.Approximately(maxValue, happiness))
            return 0; // happy
        else if (Mathf.Approximately(maxValue, anger))
            return 1; // anger
        else if (Mathf.Approximately(maxValue, regret))
            return 2; // sad

        
        return -1;    
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
    
    public override int GetHashCode() => (anger, suspicion, happiness, regret).GetHashCode();
    
    public override bool Equals(object obj)
    {
        if (obj is not EmotionalValue other) return false;

        return Mathf.Approximately(anger, other.anger) &&
               Mathf.Approximately(suspicion, other.suspicion) &&
               Mathf.Approximately(happiness, other.happiness) &&
               Mathf.Approximately(regret, other.regret);
    }
}