using UnityEngine;


public class EmotionalValue
{
    public float anger { get; private set; }
    public float suspicion { get; private set; } // Suspicion
    public float happiness { get; private set; }
    public float regret { get; private set; }

    public EmotionalValue(float anger = 0f, float consciousness = 0f, float happiness = 0f, float regret = 0f)
    {
        this.anger = anger;
        suspicion = consciousness;
        this.happiness = happiness;
        this.regret = regret;
    }

    public void Add(EmotionalValue other)
    {
        anger += other.anger;
        suspicion += other.suspicion;
        happiness += other.happiness;
        regret += other.regret;
    }

    public void ClampValues(float max = 1f)
    {
        anger = Mathf.Clamp(anger, 0, max);
        suspicion = Mathf.Clamp(suspicion, 0, max);
        happiness = Mathf.Clamp(happiness, 0, max);
        regret = Mathf.Clamp(regret, 0, max);
    }

    public string GetDominantEmotion()
    {
        float max = Mathf.Max(anger, suspicion, happiness, regret);
        if (Mathf.Approximately(max, anger)) return "Anger";
        if (Mathf.Approximately(max, suspicion)) return "Consciousness";
        if (Mathf.Approximately(max, happiness)) return "Happiness";
        return "Regret";
    }

    public void Normalize()
    {
        float total = anger + suspicion + happiness + regret;
        if (total == 0) return;
        anger /= total;
        suspicion /= total;
        happiness /= total;
        regret /= total;
    }
}
