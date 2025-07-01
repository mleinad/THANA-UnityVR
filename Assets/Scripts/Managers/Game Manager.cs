using System;
using MemoryLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LightManager lightManager; // or find dynamically
        [SerializeField] private MemoryManager memoryManager;
        [SerializeField] private GameObject city;

        private ILightManager _lightManager;
        private IMemoryManager _memoryManager;

        private bool c;
        private async void Start()
        {
            _lightManager = lightManager;
            _memoryManager = memoryManager;
            
            MemoryResultData.Instance.SetEnding(1);
            
            //lighting abstractions 
            var lightProvider = new SceneLightProvider();
            var emotionResolver = new EmotionColorResolver(
                angerThreshold: 0.2f, happinessThreshold: 0.2f, regretThreshold: 0.2f,
                angerColor: Color.red, happinessColor: Color.yellow, regretColor: Color.blue
            );
            var transitionEffect = new LightTransitionEffect(lightProvider, transitionSpeed: 2f, intensityMultiplier: 1f);
            var flickerEffect = new LightFlickerEffect(lightProvider, flickerRange: 0.5f, timeBetweenIntensity: 0.1f);



            await _memoryManager.InitializeAsync();
            
            await _lightManager.InitializeAsync(lightProvider, emotionResolver, transitionEffect, flickerEffect);
            
            
            //after initialization
            SetRoomState();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                city.SetActive(c);
                c = !c;
            }
        }

        private void SetRoomState()
        {
            _memoryManager.RecalculateEmotions();
        }
    }
}
