using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interactions
{
    public class DisturbDetection : MonoBehaviour
    {
        [Header("Launch Detection")] [SerializeField]
        private float launchForceThreshold = 2.0f;

        [Header("Drop Detection")] [SerializeField]
        private float dropHeightThreshold = 1.0f;

        [Header("Debug")] [SerializeField] private bool debug = true;

        private XRGrabInteractable grabInteractable;
        private Rigidbody rb;

        private float grabStartY = 0f;

        public static event Action<GameObject> OnDisturbDetected;

        private void Awake()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            grabStartY = transform.position.y;

            if (debug)
                Debug.Log($"[Grabbed] Start Y: {grabStartY}");
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            float releaseSpeed = rb.velocity.magnitude;
            float releaseY = transform.position.y;
            float heightDrop = Mathf.Abs(releaseY - grabStartY);

            if (debug)
            {
                Debug.Log($"[Released] Speed: {releaseSpeed}");
                Debug.Log($"[Released] Height drop: {heightDrop}");
            }

            if (releaseSpeed >= launchForceThreshold)
            {
                OnLaunched(releaseSpeed);
            }
            else if (heightDrop >= dropHeightThreshold)
            {
                OnDropped(heightDrop);
            }
        }
        private void OnLaunched(float force)
        {
                if (debug)
                    Debug.Log($"[LaunchDetection] Object launched with force: {force}");
                if (OnDisturbDetected != null) OnDisturbDetected.Invoke(gameObject);
        }
        
        private void OnDropped(float height)
        {
            if (debug)
                Debug.Log($"[LaunchDetection] Object dropped from height: {height}");
            if (OnDisturbDetected != null) OnDisturbDetected.Invoke(gameObject);
        }
    }
}
