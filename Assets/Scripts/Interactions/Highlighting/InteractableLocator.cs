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
        
        public Transform[] GetRightHandInteractableProximityList()
        {
            var array = GetOrderedInteractables(rightHand);
            
            return array.Length > 0 ? array.Select(x => x.transform).ToArray() : null;
        }

        public Transform[]  GetLeftHandInteractableProximityList()
        {
            var array = GetOrderedInteractables(leftHand);
            return array.Length > 0 ? array.Select(x => x.transform).ToArray() : null;
        }
        
        public XRGrabInteractable[] GetOrderedInteractables(Transform hand)
        {
            Collider[] hits = Physics.OverlapSphere(hand.position, searchRadius, interactableLayer);

            return hits
                .Select(hit => hit.GetComponent<XRGrabInteractable>())
                .Where(x => x != null)
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