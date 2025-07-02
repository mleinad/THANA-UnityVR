using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MemoryLogic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LightManager lightManager; // or find dynamically
        [SerializeField] private MemoryManager memoryManager;
        [SerializeField] private GameObject city;
        [SerializeField] private GameObject exitText;
        [SerializeField] private GameObject introText;
        
        
        [SerializeField] private GameObject _teleportationProvider;
        [SerializeField] private GameObject _continuousMoveProvider;
        [SerializeField] private GameObject _snapTurnProvider;
        [SerializeField] private GameObject _smoothTurnProvider;
        
        
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
            var flickerEffect = new LightFlickerEffect(lightProvider);

            exitText.SetActive(false);

            await _memoryManager.InitializeAsync();
            
            await _lightManager.InitializeAsync(lightProvider, emotionResolver, transitionEffect, flickerEffect);
            
            
            //after initialization
            SetRoomState();
            
            SetXRSettings();
            
            OnStartText();
        }


        private void SetXRSettings()
        {
            if (MemoryResultData.Instance.GetXrTurn())
            {
                //snap mode
                _snapTurnProvider.SetActive(true);
                _smoothTurnProvider.SetActive(false);
            }
            else
            {
                //smooth turn
                _snapTurnProvider.SetActive(false);
                _smoothTurnProvider.SetActive(true);
            }

            if (MemoryResultData.Instance.GetXrMove())
            {
                //tp
                _teleportationProvider.SetActive(true);
                _smoothTurnProvider.SetActive(false);
            }
            else
            {
                _teleportationProvider.SetActive(false);
                _smoothTurnProvider.SetActive(true);
                //smooth
            }
        }

        private async void OnStartText()
        {
            introText.SetActive(true);
            await UniTask.Delay(5000); // 5000 milliseconds = 5 seconds
            introText.SetActive(false);
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
            
            exitText.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3));

            int emotion = _memoryManager.GetFinalValue().GetDominantEnding();


            if (MemoryResultData.Instance != null)
            {
                MemoryResultData.Instance.SetEnding(emotion);
            }

            SceneManager.LoadScene("Intro");
        }
    }
}
