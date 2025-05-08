using System;
using System.Collections.Generic;
using UnityEngine;

public class MemoryDocument: MonoBehaviour, IMemoryModifier
{
    [SerializeField]
    List<GameObject> alternativeObjects;

    private void Awake()
    {
        ToggleVisibility(0);
    }

    public EmotionalValue GetEmotionalImpact()
    {
        throw new NotImplementedException();
    }

    public int ToggleVisibility(int activeIndex)
    {
        foreach (var obj in alternativeObjects) { obj.SetActive(false); }
        alternativeObjects[activeIndex].gameObject.SetActive(true);
      
        return alternativeObjects.Count;
    }
}
