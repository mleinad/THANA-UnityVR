using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip suspiciousClip;
    [SerializeField] private AudioClip happinessClip;
    [SerializeField] private AudioClip regretClip;

    [Header("Audio Settings")]
    [SerializeField, Range(0f, 1f)] private float volumeMultiplier = 1f;
    [SerializeField] private float fadeSpeed = 1f;

    private Dictionary<string, AudioSource> emotionSources = new();
    private float targetSuspiciousVol, targetHappinessVol, targetRegretVol;

    private void Awake()
    {
        CreateAudioSources();
        StartAllLoops();
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
        BlendVolumes();
    }

    private void CreateAudioSources()
    {
        emotionSources["suspicious"] = CreateLoopingSource(suspiciousClip);
        emotionSources["happiness"] = CreateLoopingSource(happinessClip);
        emotionSources["regret"] = CreateLoopingSource(regretClip);
    }

    private AudioSource CreateLoopingSource(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = 0f;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        return source;
    }

    private void StartAllLoops()
    {
        foreach (var src in emotionSources.Values)
        {
            if (src.clip != null)
                src.Play();
        }
    }

    private void OnEmotionsChanged(EmotionalValue emotions)
    {
        // Normalize emotional weights (focus on dominant blend)
        float total = emotions.suspicion + emotions.happiness + emotions.regret;

        if (total > 0f)
        {
            targetSuspiciousVol = (emotions.suspicion / total) * volumeMultiplier;
            targetHappinessVol = (emotions.happiness / total) * volumeMultiplier;
            targetRegretVol = (emotions.regret / total) * volumeMultiplier;
        }
        else
        {
            targetSuspiciousVol = targetHappinessVol = targetRegretVol = 0f;
        }
    }

    private void BlendVolumes()
    {
        emotionSources["suspicious"].volume = Mathf.MoveTowards(emotionSources["suspicious"].volume, targetSuspiciousVol, fadeSpeed * Time.deltaTime);
        emotionSources["happiness"].volume = Mathf.MoveTowards(emotionSources["happiness"].volume, targetHappinessVol, fadeSpeed * Time.deltaTime);
        emotionSources["regret"].volume = Mathf.MoveTowards(emotionSources["regret"].volume, targetRegretVol, fadeSpeed * Time.deltaTime);
    }
}
