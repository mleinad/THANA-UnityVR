using UnityEngine;

namespace Interactions.Interfaces
{
    public interface IProximityHighlightController
    {
        public Transform GetClosestObjectLeft();
        public Transform GetClosestObjectRight();
    }
}