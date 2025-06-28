using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LightManager lightManager; // or find dynamically
        [SerializeField] private MemoryManager memoryManager;
        
        private ILightManager _lightManager;
        private IMemoryManager _memoryManager;

        private async void Start()
        {
            _lightManager = lightManager;
            _memoryManager = memoryManager;
            
            
            //lighting abstractions 
            var lightProvider = new SceneLightProvider();
            var emotionResolver = new EmotionColorResolver(
                angerThreshold: 0.7f, happinessThreshold: 0.7f, regretThreshold: 0.7f,
                angerColor: Color.red, happinessColor: Color.yellow, regretColor: Color.blue
            );
            var transitionEffect = new LightTransitionEffect(lightProvider, transitionSpeed: 2f, intensityMultiplier: 1f);
            var flickerEffect = new LightFlickerEffect(lightProvider, flickerRange: 0.5f, timeBetweenIntensity: 0.1f);



            await _memoryManager.InitializeAsync();
            
            await _lightManager.InitializeAsync(lightProvider, emotionResolver, transitionEffect, flickerEffect);
            
            
            //after initialization
            SetRoomState();
        }

        private void SetRoomState()
        {
            _memoryManager.RecalculateEmotions();
        }
    }
}
