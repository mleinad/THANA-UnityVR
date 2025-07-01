using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private float fillSpeed = 0.5f;

        [SerializeField]
        private SceneGroup[] sceneGroup;

        private float targetProgress;
        private float currentProgress;
        private bool isLoading;

        public readonly SceneGroupManager manager = new SceneGroupManager();
        
        public static SceneLoader Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            manager.OnSceneLoaded += sceneName => Debug.Log($"Loaded: {sceneName}");
            manager.OnSceneUnloaded += sceneName => Debug.Log($"Unloaded: {sceneName}");
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group fully loaded");
        }

        private async void Start()
        {
            await LoadSceneGroup(0); // Load initial scene group (e.g. main menu)
        }

        private void Update()
        {
            if (!isLoading) return;

            float progressDifference = Mathf.Abs(currentProgress - targetProgress);
            float dynamicFillSpeed = progressDifference * fillSpeed;

            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * dynamicFillSpeed);

            Debug.Log($"Loading Progress: {currentProgress:P0}");
        }

        public async Task LoadSceneGroup(int index)
        {
            if (index < 0 || index >= sceneGroup.Length)
            {
                Debug.LogError("Invalid scene group index: " + index);
                return;
            }

            currentProgress = 0f;
            targetProgress = 0f;
            isLoading = true;

            var progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);

            await manager.LoadScene(sceneGroup[index], progress);

            isLoading = false;
            Debug.Log("Finished loading scene group.");
        }
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        private const float ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}
