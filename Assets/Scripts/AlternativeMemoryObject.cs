using UnityEngine;


public class AlternativeMemoryObject : MonoBehaviour
{
    public GameObject variant;
    public EmotionalValue emotions = new EmotionalValue();
    public bool isDefault = false;

    private ObjectSwap _parent;
    
    [SerializeField] private bool _isSelected;
    public bool isSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            if (_isSelected)
            {
                _parent?.OnVariantSelected(this);
            }
        }
    }

    private void Awake()
    {
        _parent = GetComponentInParent<ObjectSwap>();
        isSelected = isDefault;
    }

    public void Activate() => variant.SetActive(true);
    public void Deactivate() => variant.SetActive(false);
    
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_isSelected)
        {
            var parentSwap = GetComponentInParent<ObjectSwap>();
            if (parentSwap == null) return;

            foreach (var alt in parentSwap.GetComponentsInChildren<AlternativeMemoryObject>())
            {
                if (alt != this)
                {
                    alt._isSelected = false;
                    UnityEditor.EditorUtility.SetDirty(alt);
                }
            }
        }
    }
#endif
}