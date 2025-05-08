using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.Serialization;

public class MemoryManager : MonoBehaviour
{
    [RequireInterface(typeof(IMemoryModifier))]
    public List<UnityEngine.Object> memoryModifiers;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GetMemoryValue()
    {
        foreach (var m in memoryModifiers)
        {
          
        }
    }
}
