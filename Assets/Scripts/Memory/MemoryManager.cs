using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.Serialization;

public class MemoryManager : MonoBehaviour
{
    [RequireInterface(typeof(IMemoryModifier))]
    public List<UnityEngine.Object> memoryModifiers;
    
    EmotionalValue emotionalValue;
    
    // Start is called before the first frame update
    void Start()
    {
        emotionalValue = new EmotionalValue();
    }
    
    void GetMemoryValue()
    {
        
        foreach (IMemoryModifier m in memoryModifiers)
        {
            emotionalValue.Add(m.GetEmotionalImpact());
        }
    }
}
