using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowableMemoryObject : MonoBehaviour, ISuspicionOnlyModifier
    {
        [Range(0f, 1f)] public float suspicionBoost;

        public EmotionalValue GetEmotionalImpact()
        {
            return new EmotionalValue
            {
                anger = 0f,
                happiness = 0f,
                regret = 0f,
                suspicion = suspicionBoost
            };
        }
    }
