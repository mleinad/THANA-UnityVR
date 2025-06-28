using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interactions.Interfaces;
using UnityEngine;

namespace Interactions
{
    public class ProximityHighlighterController : MonoBehaviour, IProximityHighlightController
    {
        [SerializeField] private InteractableLocator _locator;
        private HashSet<Transform> _previousTargets = new();
        private const float UpdateIntervalSeconds = 0.2f;
       
        private void Start() => RunHighlightLoop().Forget();

        private async UniTaskVoid RunHighlightLoop()
        {
            while (true)
            {
                var currentTargets = new HashSet<Transform>(_locator.GetLeftHandInteractableProximityList());
                float count = currentTargets.Count;
                int i = 0;

                // Disable highlights for targets no longer in range
                foreach (var oldTarget in _previousTargets)
                {
                    if (!currentTargets.Contains(oldTarget))
                    {
                        var h = oldTarget.GetComponent<MeshHighlightVisual>();
                        if (h != null) h.Hide();
                    }
                }

                // Update and show highlights for current targets
                foreach (var target in currentTargets)
                {
                    var h = target.GetComponent<MeshHighlightVisual>();
                    if (h == null) continue;

                    if (!h.beenInitialized)
                        h.Initialize();

                    float priority = 1f - (i / count);
                    h.UpdateVisual(priority);
                    h.Show();
                    i++;
                }

                _previousTargets = currentTargets;

                await UniTask.Delay(TimeSpan.FromSeconds(UpdateIntervalSeconds));
            }
        }

        
        public Transform GetClosestObjectLeft()
        {
            throw new NotImplementedException();
        }

        public Transform GetClosestObjectRight()
        {
            throw new NotImplementedException();
        }
    }
}