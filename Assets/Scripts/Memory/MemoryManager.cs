using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.Serialization;

public class MemoryManager : MonoBehaviour
{
    [RequireInterface(typeof(IMemoryModifier))]
    public List<UnityEngine.Object> memoryModifiers;
    
<<<<<<< HEAD
    EmotionalValue emotionalValue;
=======
>>>>>>> d8e11d5185d46293cd07b8d4266baa1168d51337
    
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        emotionalValue = new EmotionalValue();
    }
    
    void GetMemoryValue()
    {
        
        foreach (IMemoryModifier m in memoryModifiers)
        {
            emotionalValue.Add(m.GetEmotionalImpact());
=======
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GetMemoryValue()
    {
        foreach (var m in memoryModifiers)
        {
          
>>>>>>> d8e11d5185d46293cd07b8d4266baa1168d51337
        }
    }
}
