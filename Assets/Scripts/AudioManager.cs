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

    [Header("Audio Settings")]
    [SerializeField, Range(0f, 1f)] private float volumeMultiplier = 1f;
    [SerializeField] private float fadeSpeed = 1f;

    private enum Emotion { Suspicion, Happiness, Regret, None }

    private Dictionary<Emotion, AudioSource> emotionSources = new();
    private Emotion currentDominant = Emotion.None;
    private Emotion previousDominant = Emotion.None;

    private void Awake()
    {
        emotionSources[Emotion.Suspicion] = CreateLoopingSource(suspiciousClip);
        emotionSources[Emotion.Happiness] = CreateLoopingSource(happinessClip);
        emotionSources[Emotion.Regret] = CreateLoopingSource(regretClip);
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

    private void Update()
    {
        UpdateVolumes();
    }

    private AudioSource CreateLoopingSource(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = 0f;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        if (clip != null)
            source.Play();
        return source;
    }

    private void OnEmotionsChanged(EmotionalValue emotions)
    {
        // Determine the dominant emotion
        float suspicion = emotions.suspicion;
        float happiness = emotions.happiness;
        float regret = emotions.regret;

        currentDominant = GetDominantEmotion(suspicion, happiness, regret);
    }

    private Emotion GetDominantEmotion(float suspicion, float happiness, float regret)
    {
        if (suspicion <= 0f && happiness <= 0f && regret <= 0f)
            return Emotion.None;

        if (suspicion >= happiness && suspicion >= regret)
            return Emotion.Suspicion;
        if (happiness >= suspicion && happiness >= regret)
            return Emotion.Happiness;
        return Emotion.Regret;
    }

    private void UpdateVolumes()
    {
        foreach (var pair in emotionSources)
        {
            var emotion = pair.Key;
            var source = pair.Value;

            float targetVol = (emotion == currentDominant) ? volumeMultiplier : 0f;
            source.volume = Mathf.MoveTowards(source.volume, targetVol, fadeSpeed * Time.deltaTime);
        }
    }
}
