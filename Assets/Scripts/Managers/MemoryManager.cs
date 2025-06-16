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
        [SerializeField, Range(0f, 1f)] private float suspicionThreshold = 0.8f;
        

        public event Action<EmotionalValue> EmotionalStateChanged;


        private EmotionalValue CurrentEmotions { get; set; } = new EmotionalValue();
        private EmotionalValue _sceneEmotionalValue = new EmotionalValue();
        
        public static MemoryManager Instance;
        
        private void OnDisable() {
            ObjectSwap.OnAnySelectionChanged -= RecalculateEmotions;
            DisturbDetection.OnDisturbDetected -= OnDisturbDetected;
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
            DisturbDetection.OnDisturbDetected += OnDisturbDetected;

            RecalculateEmotions();
        }

        private void RecalculateEmotions()
        {
            Debug.Log("RecalculateEmotions...");
            if (memoryModifiers == null || memoryModifiers.Count == 0) return;

            EmotionalValue regularTotal = new EmotionalValue();
            int regularCount = 0;

            foreach (var obj in memoryModifiers)
            {
                if (obj is IMemoryModifier modifier)
                {
                    EmotionalValue impact = modifier.GetEmotionalImpact();

                        regularTotal += impact;
                        regularCount++;
                }
            }

            if (regularCount == 0) return;

            EmotionalValue average = regularTotal / regularCount;

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


        private void OnDisturbDetected(GameObject disturbedObject)
        {
            if (disturbedObject.TryGetComponent<ThrowableMemoryObject>(out var throwableObject))
            {
                CurrentEmotions.suspicion += throwableObject.suspicionBoost;
                CurrentEmotions.suspicion = Mathf.Clamp01(CurrentEmotions.suspicion); // Ensure suspicion stays within 0-1 range
                EmotionalStateChanged?.Invoke(CurrentEmotions);
                DebugValues();

                if (CurrentEmotions.suspicion >= suspicionThreshold)
                {
                    Debug.Log("Losing condition triggered: Suspicion too high!");
                }
            }
        }
    }
}
