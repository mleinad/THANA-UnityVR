using UnityEngine;

public class EmotionLight: MonoBehaviour
{
        public Light targetLight;
        public EmotionalValue emotionSource;
        
        private void Update()
        {
            if (targetLight == null || emotionSource == null) return;

       
        }
}
