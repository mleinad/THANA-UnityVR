# T.H.A.N.A. – VR Game Project

**Project Type:** Unity VR

**Genre:** Sci‑Fi / Psychological / Simulation

---

## Overview

*T.H.A.N.A.* is a VR game where the player takes on the role of an AI named **T.H.A.N.A.**, capable of infiltrating and manipulating human memories. The goal is to subtly alter a character's perception of their environment and experiences by modifying their memories and emotional states.

The game combines immersive VR interaction with deep psychological mechanics, emphasizing the emotional impact of actions and memory alteration.

This project was developed during my Erasmus semester at the **University of Applied Sciences Upper Austria** and in collaboration with a dedicated **3D artist** and **Game Designer**, who developed the narrative and a number of custom assets used in the prototype.

The game uses a **bootloader architecture**, ensuring modular scene loading and consistent initialization of core VR systems and gameplay services.

---

## Story

Set in a dystopian sci‑fi world, T.H.A.N.A. operates within human neural networks to influence key individuals. The AI must carefully balance emotional cues to guide characters’ decisions and perspectives without being detected.

Players manipulate objects, memories, and environmental cues to achieve desired outcomes, observing the cascading effects on a character’s emotional state and behavior.

The game features **multiple endings**, which change depending on the player's actions, choices, and how they influence the characters' emotional states throughout the gameplay.

---

## Core Mechanics

### Emotional Value System

Memories and objects in the game carry **emotional values** representing different emotional states:

* **Anger**
* **Suspicion**
* **Happiness**
* **Regret**

Each `EmotionalValue` can be combined and averaged to compute the overall emotional impact on the character.

#### `EmotionalValue` Class Example

```csharp
[System.Serializable]
public class EmotionalValue
{
    public float Anger;
    public float Suspicion;
    public float Happiness;
    public float Regret;

    public static EmotionalValue operator +(EmotionalValue a, EmotionalValue b)
    {
        return new EmotionalValue
        {
            Anger = a.Anger + b.Anger,
            Suspicion = a.Suspicion + b.Suspicion,
            Happiness = a.Happiness + b.Happiness,
            Regret = a.Regret + b.Regret
        };
    }

    public static EmotionalValue operator /(EmotionalValue a, float divisor)
    {
        return new EmotionalValue
        {
            Anger = a.Anger / divisor,
            Suspicion = a.Suspicion / divisor,
            Happiness = a.Happiness / divisor,
            Regret = a.Regret / divisor
        };
    }

    public static EmotionalValue Average(List<EmotionalValue> values)
    {
        EmotionalValue sum = new EmotionalValue();
        foreach (var val in values) sum += val;
        return sum / values.Count;
    }
}
```

### Memory Alteration Mechanics

Objects in the game can have **alternative memory states**, each with its own emotional values. Players can swap objects’ states to manipulate the character’s memory and perception.

#### Example Usage in Game

```csharp
public class ObjectSwap : MonoBehaviour
{
    public AlternativeMemoryObject currentObject;

    public void SwapTo(AlternativeMemoryObject alternative)
    {
        currentObject = alternative;
        ApplyEmotionalImpact(currentObject.EmotionalValues);
    }

    private void ApplyEmotionalImpact(EmotionalValue values)
    {
        character.Emotions += values;
    }
}
```

---

## Editor Tools

To streamline development and content creation, custom Unity editor tools were created:

1. **Alternative Memory Object Editor**

   * Allows designers to define multiple variants of objects.
   * Assign emotional values to each variant.
   * Preview the emotional impact in real‑time.

2. **Emotional Value Inspector**

   * Custom property drawer for `EmotionalValue`.
   * Shows intuitive sliders for each emotion type.
   * Calculates and displays averages dynamically.

3. **Memory Swap Debugger**

   * Lets developers simulate object swaps in the editor.
   * Tracks cumulative emotional impact on characters.
   * Visualizes potential outcomes without running full VR simulation.

#### Example: Custom Inspector Snippet

```csharp
[CustomEditor(typeof(AlternativeMemoryObject))]
public class AlternativeMemoryObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AlternativeMemoryObject obj = (AlternativeMemoryObject)target;

        EditorGUILayout.LabelField("Emotional Values", EditorStyles.boldLabel);
        obj.EmotionalValues.Anger = EditorGUILayout.Slider("Anger", obj.EmotionalValues.Anger, 0f, 100f);
        obj.EmotionalValues.Happiness = EditorGUILayout.Slider("Happiness", obj.EmotionalValues.Happiness, 0f, 100f);
        obj.EmotionalValues.Suspicion = EditorGUILayout.Slider("Suspicion", obj.EmotionalValues.Suspicion, 0f, 100f);
        obj.EmotionalValues.Regret = EditorGUILayout.Slider("Regret", obj.EmotionalValues.Regret, 0f, 100f);

        if (GUILayout.Button("Preview Emotional Impact"))
        {
            Debug.Log("Previewing emotional impact: " + obj.EmotionalValues);
        }
    }
}
```

---

## Summary

*T.H.A.N.A.* combines VR immersion with complex emotional and psychological mechanics, allowing players to interact with and manipulate memories to influence characters. Its development emphasizes:

* **Dynamic memory and object swapping**
* **Emotional value simulation**
* **Custom editor tools for rapid content iteration**
* **Multiple endings based on player choices and actions**
* **Bootloader architecture for modular scene and system management**
* **Unity XR development toolkit**

This project demonstrates a fusion of narrative‑driven gameplay and sophisticated system design, all within a VR environment.

---

