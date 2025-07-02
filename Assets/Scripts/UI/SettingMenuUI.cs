using Interactions;
using MemoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace UI
{
    public class SettingMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button teleportButton;
        [SerializeField] private Button continuosMoveButton;
        
        [SerializeField] private Button smoothTurnButton;
        [SerializeField] private Button snapTurnButton;

        [Header("Colors")]
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.gray;

        private bool snapTurn, teleport;

        private void Start()
        {
            teleportButton.onClick.AddListener(EnableTeleportMode);
            continuosMoveButton.onClick.AddListener(EnableContinuousMoveMode);
            snapTurnButton.onClick.AddListener(EnableSnapTurnMode);
            smoothTurnButton.onClick.AddListener(EnableSmoothTurnMode);

            EnableSnapTurnMode();
            EnableContinuousMoveMode();
        }

        private void EnableTeleportMode()
        {
            SetButtonColors(teleportButton, continuosMoveButton);
            teleport = true;
            MemoryResultData.Instance.SetXRSettings(snapTurn, teleport);
        }

        private void EnableContinuousMoveMode()
        {
            SetButtonColors(continuosMoveButton, teleportButton);
            teleport = false;
            MemoryResultData.Instance.SetXRSettings(snapTurn, teleport);
        }

        private void EnableSnapTurnMode()
        {
            SetButtonColors(snapTurnButton, smoothTurnButton);
            snapTurn = true;
            MemoryResultData.Instance.SetXRSettings(snapTurn, teleport);
        }

        private void EnableSmoothTurnMode()
        {
            SetButtonColors(smoothTurnButton, snapTurnButton);
            snapTurn = false;
            MemoryResultData.Instance.SetXRSettings(snapTurn, teleport);
        }

        private void SetButtonColors(Button active, Button inactive)
        {
            if (active.TryGetComponent(out Image activeImage))
                activeImage.color = activeColor;
            if (inactive.TryGetComponent(out Image inactiveImage))
                inactiveImage.color = inactiveColor;
        }
    }
}
