using System.Threading;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

public class LightManager : MonoBehaviour, ILightManager
{
    #region Dependencies

    private ISceneLightProvider _lightProvider;
    private IEmotionColorResolver _emotionColorResolver;
    private ILightTransitionEffect _transitionEffect;
    private ILightFlickerEffect _flickerEffect;

    #endregion

    #region State

    private float _suspicionMultiplier;
    private CancellationTokenSource _transitionCts;

    #endregion

    #region Initialization

    public async UniTask InitializeAsync(
        ISceneLightProvider lightProvider,
        IEmotionColorResolver emotionColorResolver,
        ILightTransitionEffect transitionEffect,
        ILightFlickerEffect flickerEffect)
    {
        _lightProvider = lightProvider;
        _emotionColorResolver = emotionColorResolver;
        _transitionEffect = transitionEffect;
        _flickerEffect = flickerEffect;

        await _lightProvider.InitializeAsync();

      //  await UniTask.WaitUntil(() => MemoryManager.Instance != null);
        MemoryManager.Instance.EmotionalStateChanged += OnEmotionsChanged;

        Debug.Log("LightManager initialized");
    }
    

    #endregion

    #region Emotion Response

    private void OnEmotionsChanged(EmotionalValue emotions)
    {

        Color color = _emotionColorResolver.GetDominantColor(emotions, out float strength);
        
        _suspicionMultiplier = emotions.suspicion >= 0.3f ? emotions.suspicion : 0f;

        _transitionCts?.Cancel();
        _transitionCts = new CancellationTokenSource();

        _transitionEffect.SmoothTransitionAsync(color, strength, _transitionCts.Token).Forget();
    }

    #endregion

    #region Update Loop

    private void Update()
    {
        if (_suspicionMultiplier > 0f)
        {
            _flickerEffect.FlickerLights(_suspicionMultiplier);
        }
    }

    #endregion

    #region Cleanup

    private void OnDisable()
    {
        if (MemoryManager.Instance != null)
            MemoryManager.Instance.EmotionalStateChanged -= OnEmotionsChanged;

        _transitionCts?.Cancel();
        _transitionCts?.Dispose();
    }

    #endregion
}
