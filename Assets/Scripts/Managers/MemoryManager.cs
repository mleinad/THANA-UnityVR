using System;
using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Interactions;
using UnityEngine;

namespace Managers
{
    public class MemoryManager : MonoBehaviour
    {
        [RequireInterface(typeof(IMemoryModifier))]
        public List<UnityEngine.Object> memoryModifiers;


        [Header("Current Emotional Status")]
      
        [SerializeField, Range(0f, 1f)] private float _anger;
        [SerializeField, Range(0f, 1f)] private float _suspicion;
        [SerializeField, Range(0f, 1f)] private float _happiness;
        [SerializeField, Range(0f, 1f)] private float _regret;
        

        public event Action<EmotionalValue> EmotionalStateChanged;


        private EmotionalValue CurrentEmotions { get; set; } = new EmotionalValue();
        private EmotionalValue _sceneEmotionalValue = new EmotionalValue();
        
        public static MemoryManager Instance;
        
        private void OnDisable() {
            ObjectSwap.OnAnySelectionChanged -= RecalculateEmotions;
     //       DisturbDetection.OnDisturbDetected -= RecalculateEmotions;
        }


        private async void Awake()
        {
            await Initialize();
        }

        public async UniTask Initialize()
        {
            await UniTask.Yield();

            Instance = this;
            CurrentEmotions.Empty();
            _sceneEmotionalValue.Empty();

            ObjectSwap.OnAnySelectionChanged += RecalculateEmotions;
          //  DisturbDetection.OnDisturbDetected -= RecalculateEmotions;

            RecalculateEmotions();
        }

        private void RecalculateEmotions()
        {
            Debug.Log("RecalculateEmotions...");
            if (memoryModifiers == null || memoryModifiers.Count == 0) return;

            EmotionalValue regularTotal = new EmotionalValue();
            float suspicionOnlyTotal = 0f;
            int regularCount = 0;

            foreach (var obj in memoryModifiers)
            {
                if (obj is IMemoryModifier modifier)
                {
                    EmotionalValue impact = modifier.GetEmotionalImpact();

                    if (modifier is ISuspicionOnlyModifier)
                    {
                   //     suspicionOnlyTotal += impact.suspicion;
                    }
                    else
                    {
                        regularTotal += impact;
                        regularCount++;
                    }
                }
            }

            if (regularCount == 0) return;

            EmotionalValue average = regularTotal / regularCount;
           // average.suspicion += suspicionOnlyTotal;

            CurrentEmotions = average;
            EmotionalStateChanged?.Invoke(CurrentEmotions);

            DebugValues();
        }

        

        private void DebugValues()
        {
            _anger     =     CurrentEmotions.anger;
            _suspicion =     CurrentEmotions.suspicion;
            _happiness =     CurrentEmotions.happiness;
            _regret    =     CurrentEmotions.regret;
        }
    }
}
