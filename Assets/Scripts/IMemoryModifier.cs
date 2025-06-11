using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemoryModifier
{
    EmotionalValue GetEmotionalImpact();
}

public interface ISuspicionOnlyModifier : IMemoryModifier
{
    // Marker interface – no methods needed
}
