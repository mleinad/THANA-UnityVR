using UnityEngine;

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

        public bool isBeingHeld;
        
        private bool inRange;
        
        private Color _originalColor;
        public void Initialize()
        {
            if (_highlightCopy != null) return;
            
            _highlightCopy = null;
            _highlightCopy = new GameObject($"{transform.name}-highlight");
            _highlightCopy.SetActive(false);

            MeshFilter meshFilter = _highlightCopy.AddComponent<MeshFilter>();
            
            _renderer = _highlightCopy.AddComponent<MeshRenderer>();
            _renderer.material = new Material(highlightMaterial);

            MeshFilter localMesh = transform.GetComponent<MeshFilter>();


            Transform copyParent = null;
            
            if (localMesh != null) //has mesh
            {
                meshFilter.mesh = localMesh.mesh;
                copyParent = transform;
            }
            else
            {
                ObjectSwap swapper = transform.GetChild(0).GetComponent<ObjectSwap>();
                if (swapper != null)
                {
                    GameObject obj = swapper.GetCurrentGameObject();
                    meshFilter.mesh = obj.GetComponent<MeshFilter>().mesh;
                    ObjectSwap.OnAnySelectionChanged += ReInitialize;
                    copyParent = obj.transform;
                }
                else
                {
                    Debug.LogError($"Could not find MeshFilter for {transform.name}");
                }
            }

            if (copyParent != null)
            {
                _highlightCopy.transform.SetPositionAndRotation(copyParent.position, copyParent.rotation);
                _highlightCopy.transform.localScale = copyParent.localScale * (1f + scaleFactor);
                _highlightCopy.transform.SetParent(copyParent);

                _originalAlpha = _renderer.material.color.a;
                _originalScale = _highlightCopy.transform.localScale;
                beenInitialized = true;
            }
            else
            {
                Debug.LogError($"Could not find MeshFilter for {transform.name}");
            }
            
            _originalColor = _renderer.material.color;
        }

        public void UpdateVisual(float priority)
        {
            this.priority = priority;
            
            if(inRange) return;
            
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
        
        public void ReInitialize()
        {
            // Destroy the old highlight copy if it exists
            if (_highlightCopy != null)
            {
                Destroy(_highlightCopy);
                _highlightCopy = null;
                beenInitialized = false;
            }
            
            ObjectSwap.OnAnySelectionChanged -= ReInitialize;

            Initialize();
        }


        public void SetInRange(bool state)
        {

            inRange = state;
            Color color = Color.yellow;
            color.a = _originalAlpha * priority;
            
            if (state)
            {
                _renderer.material.color = color;
            }
            else
            {
                _renderer.material.color = _originalColor;
            }
        }
        
    }
}
