using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectSwap : MonoBehaviour, IMemoryModifier
    {
        [SerializeField]
        private List<AlternativeMemoryObject> alternatives = new List<AlternativeMemoryObject>();
       
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

        public EmotionalValue GetEmotionalImpact()
        {
            foreach (var alternative in alternatives.Where(alternative => alternative.isSelected))
            {
                return alternative.emotions;
            }

            return new EmotionalValue();
        }

        public void ActivateVariant(int index)
        {
            selectedIndex = index;
            foreach (var alt in alternatives)
            {
                alt.Deactivate();
            }
            alternatives[index].Activate();
        }

        
        public void OnVariantSelected(AlternativeMemoryObject selected)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                var alt = alternatives[i];
                if (alt != selected)
                {
                    alt.isSelected = false;
                    alt.Deactivate();
                }
                else
                {
                    selectedIndex = i;
                    alt.Activate();
                }
            }
        }
        
        
    }