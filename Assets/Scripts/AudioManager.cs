using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip suspiciousClip;
    [SerializeField] private AudioClip happinessClip;
    [SerializeField] private AudioClip regretClip;

    private enum Emotion { Suspicion, Happiness, Regret, None }

    private Dictionary<Emotion, AudioSource> emotionSources = new();
    private Emotion currentEmotion = Emotion.None;

    private void Awake()
    {
        emotionSources[Emotion.Suspicion] = CreateSource(suspiciousClip);
        emotionSources[Emotion.Happiness] = CreateSource(happinessClip);
        emotionSources[Emotion.Regret] = CreateSource(regretClip);
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => MemoryManager.Instance != null);
        MemoryManager.Instance.EmotionalStateChanged += OnEmotionsChanged;
    }

    private void OnDisable()
    {
        if (MemoryManager.Instance != null)
            MemoryManager.Instance.EmotionalStateChanged -= OnEmotionsChanged;
    }

    private AudioSource CreateSource(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        return source;
    }

    private void OnEmotionsChanged(EmotionalValue emotions)
    {
        Emotion dominant = GetDominantEmotion(emotions);

        if (dominant == currentEmotion) return;

        // Stop current
        if (emotionSources.TryGetValue(currentEmotion, out var currentSource))
        {
            currentSource.Stop();
        }

        // Play new dominant
        if (emotionSources.TryGetValue(dominant, out var newSource) && newSource.clip != null)
        {
            newSource.Play();
        }

        currentEmotion = dominant;
    }

    private Emotion GetDominantEmotion(EmotionalValue emotions)
    {
        float suspicion = emotions.suspicion;
        float happiness = emotions.happiness;
        float regret = emotions.regret;

        float max = Mathf.Max(suspicion, happiness, regret);

        if (Mathf.Approximately(max, suspicion)) return Emotion.Suspicion;
        if (Mathf.Approximately(max, happiness)) return Emotion.Happiness;
        if (Mathf.Approximately(max, regret)) return Emotion.Regret;

        return Emotion.None;
    }
}
