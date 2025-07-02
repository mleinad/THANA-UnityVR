using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MemoryLogic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LightManager lightManager; // or find dynamically
        [SerializeField] private MemoryManager memoryManager;
        [SerializeField] private GameObject city;
        [SerializeField] private GameObject text;
        private ILightManager _lightManager;
        private IMemoryManager _memoryManager;
        
        [Header("Exit Game Button")]
        [SerializeField] private InputActionReference menuButton;

        private bool c;
        private async void Start()
        {
            _lightManager = lightManager;
            _memoryManager = memoryManager;

            if (MemoryResultData.Instance != null)
            {
                //MemoryResultData.Instance.SetEnding(1);
            }
            else
            {
                GameObject gameObj = new GameObject("memoryManagerRuntime");
                gameObj.AddComponent<MemoryResultData>();
                MemoryResultData.Instance.BeenLoaded = true;
            }
            
            //lighting abstractions 
            var lightProvider = new SceneLightProvider();
            var emotionResolver = new EmotionColorResolver(
                angerThreshold: 0.2f, happinessThreshold: 0.2f, regretThreshold: 0.2f,
                angerColor: Color.red, happinessColor: Color.yellow, regretColor: Color.blue
            );
            var transitionEffect = new LightTransitionEffect(lightProvider, transitionSpeed: 2f, intensityMultiplier: 1f);
            var flickerEffect = new LightFlickerEffect(lightProvider, flickerRange: 0.5f, timeBetweenIntensity: 0.1f);

            text.SetActive(false);

            await _memoryManager.InitializeAsync();
            
            await _lightManager.InitializeAsync(lightProvider, emotionResolver, transitionEffect, flickerEffect);
            
            
            //after initialization
            SetRoomState();
        }

        private void OnEnable()
        {
            menuButton.action.performed += TriggerEnding;
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

        void OnDisable()
        {
            menuButton.action.performed -= TriggerEnding;
        }

        void TriggerEnding(InputAction.CallbackContext context)
        {
            _= DelayedEnding();
        }

        private async Task DelayedEnding()
        {
            
            text.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3));

            int emotion = _memoryManager.GetFinalValue().GetDominantEmotion();


            if (MemoryResultData.Instance != null)
            {
                MemoryResultData.Instance.SetEnding(emotion);
            }

            SceneManager.LoadScene("Intro");
        }
    }
}
