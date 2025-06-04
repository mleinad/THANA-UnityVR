using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using Managers;
using UnityEngine;

namespace Mechanics
{
    public class MemoryManager : MonoBehaviour
    {
        [RequireInterface(typeof(IMemoryModifier))]
        public List<UnityEngine.Object> memoryModifiers;

        private EmotionalValue sceneEmotionalValue = new EmotionalValue();

        [SerializeField] private float _anger;
        [SerializeField] private float _suspicion;
        [SerializeField] private float _happiness;
        [SerializeField] private float _regret;
        
        [Header("Emotion Thresholds")]
        [SerializeField, Range(0f, 1f)] private float angerThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float happinessThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float regretThreshold = 0.7f;
        [SerializeField, Range(0f, 1f)] private float suspicionThreshold = 0.7f;

        
        public float Anger
        {
            get => _anger;
            private set
            {
                if (!Mathf.Approximately(_anger, value))
                {
                    _anger = value;
                    OnAngerChanged(_anger);
                }
            }
        }
        public float Suspicion
        {
            get => _suspicion;
            private set
            {
                if (!Mathf.Approximately(_suspicion, value))
                {
                    _suspicion = value;
                    OnSuspicionChanged(_suspicion);
                }
            }
        }
        public float Happiness
        {
            get => _happiness;
            private set
            {
                if (!Mathf.Approximately(_happiness, value))
                {
                    _happiness = value;
                    OnHappinessChanged(_happiness);
                }
            }
        }
        public float Regret
        {
            get => _regret;
            private set
            {
                if (!Mathf.Approximately(_regret, value))
                {
                    _regret = value;
                    OnRegretChanged(_regret);
                }
            }
        }
        
        
        
        private void Update()
        {
            sceneEmotionalValue.Empty();
            GetMemoryValue();

            Anger = sceneEmotionalValue.anger;
            Suspicion = sceneEmotionalValue.suspicion;
            Happiness = sceneEmotionalValue.happiness;
            Regret = sceneEmotionalValue.regret;
            
            LightManager.Instance.UpdateLightColor(Anger, Happiness, Regret);
        }

        private void GetMemoryValue()
        {
            int total = memoryModifiers.Count;
            foreach (IMemoryModifier modifier in memoryModifiers)
            {
                sceneEmotionalValue += modifier.GetEmotionalImpact() / total;
            }
        }

        #region Change Handlers
        
        private void OnAngerChanged(float value)
        {
        }

        private void OnSuspicionChanged(float value)
        {
            if (value > suspicionThreshold)
            {
                LightManager.Instance.FlickerLights(value);
            }else if (value < suspicionThreshold)
            {
                LightManager.Instance.FlickerLights(0);
            }
        }

        private void OnHappinessChanged(float value)
        {
        }

        private void OnRegretChanged(float value)
        {
        }
        
        #endregion        
    }
}
