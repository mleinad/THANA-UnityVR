using UnityEngine;

namespace MemoryLogic
{
    public class MemoryResultData : MonoBehaviour
    {
        public static MemoryResultData Instance { get; private set; }

        [SerializeField]
        public bool BeenLoaded { get; set; }
        public int Ending { get; private set; } = -1; // -1 means no ending set yet

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Destroy duplicates
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            BeenLoaded = false;
        }

        public void SetEnding(int ending)
        {
            Ending = ending;
        }
    }
}