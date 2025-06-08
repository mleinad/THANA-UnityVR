using System;
using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
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

            RecalculateEmotions();
        }

        private void RecalculateEmotions()
        {
            Debug.Log("RecalculateEmotions...");
            if (memoryModifiers == null || memoryModifiers.Count == 0) return;

            EmotionalValue total = new EmotionalValue();
            
            int count = 0;

            foreach (var obj in memoryModifiers)
            {
                if (obj is IMemoryModifier modifier) {
                    total = total + modifier.GetEmotionalImpact();
                    count++;
                }
            }

            if (count == 0) return;

            EmotionalValue average = total / count;
            
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
