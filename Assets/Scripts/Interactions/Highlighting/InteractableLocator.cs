using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interactions
{
    public class InteractableLocator : MonoBehaviour
    {
        
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;
        
        
        [SerializeField] private float searchRadius = 5f;
        [SerializeField] private LayerMask interactableLayer;
        
        [SerializeField] private List<MeshHighlightVisual> currentList;
        
        public MeshHighlightVisual[] GetRightHandInteractableProximityList()
        {
            var array = GetOrderedInteractables(rightHand);
            
            return array;
        }

        public MeshHighlightVisual[]  GetLeftHandInteractableProximityList()
        {
            var array = GetOrderedInteractables(leftHand);
            
            currentList.Clear();
            currentList = array.ToList();
            
            return array;
        }
        
        public MeshHighlightVisual[] GetOrderedInteractables(Transform hand)
        {
            Collider[] hits = Physics.OverlapSphere(hand.position, searchRadius, interactableLayer);

            return hits
                .Select(hit => hit.GetComponentInParent<MeshHighlightVisual>()) // checks parents too
                .Where(x => x != null)
                .Distinct() // optional: avoid duplicates if multiple colliders on the same object
                .OrderBy(x => Vector3.Distance(hand.position, x.transform.position))
                .ToArray();
        }

        
        /*
        private void OnDrawGizmosSelected()
        {
            if (rightHand != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(rightHand.position, searchRadius);
                
                var orderedInteractables = GetOrderedInteractables(rightHand);
                
                float max = orderedInteractables.Length == 0 ? 1 : orderedInteractables.Length;
                
                for (int i = 0; i < orderedInteractables.Length; i++)
                {
                    float strength = 1f - (i / max); // Closest = 1, farthest = near 0
                    Color c = Color.red;
                    c.a = strength;
                    Gizmos.color = c;
                    Gizmos.DrawLine(rightHand.position, orderedInteractables[i].transform.position);
                }
                
            }

            if (leftHand != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(leftHand.position, searchRadius);
               
                var orderedInteractables = GetOrderedInteractables(leftHand);
                
                float max = orderedInteractables.Length == 0 ? 1 : orderedInteractables.Length;
                
                for (int i = 0; i < orderedInteractables.Length; i++)
                {
                    float strength = 1f - (i / max); // Closest = 1, farthest = near 0
                    Color c = Color.blue;
                    c.a = strength;
                    Gizmos.color = c;
                    Gizmos.DrawLine(leftHand.position, orderedInteractables[i].transform.position);
                }
            }
        }*/
    }
}