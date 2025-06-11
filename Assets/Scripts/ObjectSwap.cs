using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectSwap : MonoBehaviour, IMemoryModifier
    {
        [SerializeField]
        private List<AlternativeMemoryObject> alternatives = new List<AlternativeMemoryObject>();
        
        public static event Action OnAnySelectionChanged;

        public int selectedIndex;
        private void Awake()
        {
            alternatives.Clear();
          
            foreach (Transform child in transform)
            {
                var altComponent = child.GetComponent<AlternativeMemoryObject>();
                if (altComponent != null)
                {
                    altComponent.variant = child.gameObject; // Assign the child as the variant
                    if(altComponent.isDefault) altComponent.Activate(); else altComponent.Deactivate();
                    alternatives.Add(altComponent);
                }
            }
        }

        public void LoadAlternatives()
        {
            foreach (var altComponent in alternatives)
            {
                var _altComponent = Instantiate(altComponent);
                _altComponent.variant = _altComponent.gameObject; // Assign the child as the variant
                if(_altComponent.isDefault) _altComponent.Activate(); else _altComponent.Deactivate();
            }
        }

        public EmotionalValue GetEmotionalImpact()
        {
            return alternatives[selectedIndex].GetEmotionalImpact();
        }

        private void ActivateVariant(int index)
        {
            selectedIndex = index;
            
            OnVariantSelected(alternatives[index]);
        }
        
        public void OnVariantSelected(AlternativeMemoryObject selected)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                var alt = alternatives[i];

                    if (alt != null)
                    {
                        
                    if (alt != selected)
                    {
                        alt.Deactivate();
                    }
                    else
                    {
                        selectedIndex = i;
                        alt.Activate();
                    }
                    OnAnySelectionChanged?.Invoke();
                }
            }
        }

        public void SwitchVariant()
        {
            selectedIndex = selectedIndex == alternatives.Count - 1 ? 0 : selectedIndex + 1;
            ActivateVariant(selectedIndex);
        }
        
        public void SwitchVariant(int direction)
        {
            if (alternatives == null || alternatives.Count == 0) return;

            selectedIndex = (selectedIndex + direction + alternatives.Count) % alternatives.Count;
            ActivateVariant(selectedIndex);
        }

        
    }