using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables;

namespace Interactions
{
    public class MeshHighlightVisual : MonoBehaviour
    {
        [SerializeField] private Material highlightMaterial;
        [SerializeField, Range(0f, 1f)] private float scaleFactor;
        [Range(0, 1)] public float priority;

        private GameObject _highlightCopy;
        private Renderer _renderer;
        private float _originalAlpha;
        private Vector3 _originalScale;
        public bool beenInitialized = false;

        public void Initialize()
        {
            if (_highlightCopy != null) return;

            _highlightCopy = new GameObject($"{transform.name}-highlight");
            _highlightCopy.SetActive(false);

            MeshFilter meshFilter = _highlightCopy.AddComponent<MeshFilter>();
            _renderer = _highlightCopy.AddComponent<MeshRenderer>();

            meshFilter.mesh = GetComponent<MeshFilter>().mesh;
            _renderer.material = new Material(highlightMaterial);

            _highlightCopy.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _highlightCopy.transform.localScale = transform.localScale * (1f + scaleFactor);
            _highlightCopy.transform.SetParent(transform);

            _originalAlpha = _renderer.material.color.a;
            _originalScale = _highlightCopy.transform.localScale;

            beenInitialized = true;
        }

        public void UpdateVisual(float priority)
        {
            this.priority = priority;
            SetOpacity();
            SetScale();
        }

        private void SetOpacity()
        {
            Color color = _renderer.material.color;
            color.a = _originalAlpha * priority;
            _renderer.material.color = color;
        }

        private void SetScale()
        {
            _highlightCopy.transform.localScale = _originalScale * (priority);
        }

        public void Show() => _highlightCopy.SetActive(true);
        public void Hide() => _highlightCopy.SetActive(false);
    }
}
