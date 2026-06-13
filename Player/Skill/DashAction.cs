using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class DashAction : MonoBehaviour
    {
        [SerializeField] private float speedVal = 0.25f;
        private InputSystem_Actions actions;
        private PlayerController playerController;

        private bool isDashRequested;
        private bool isDashing;
        private bool isShortness;

        public event Action<float> OnCurrentStamina;

        public void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            playerController = _playerController;
            actions = _actions;

            actions.Player.Dash.started += OnDashStarted;
            actions.Player.Dash.canceled += OnDashCanceled;
        }

        public void Begin()
        {
            isDashRequested = false;
            isDashing = false;
            isShortness = false;
        }

        public void Destroy()
        {
            if (actions != null)
            {
                actions.Player.Dash.started -= OnDashStarted;
                actions.Player.Dash.canceled -= OnDashCanceled;
            }
        }

        private void OnDashStarted(InputAction.CallbackContext context)
        {
            isDashRequested = true;
        }

        private void OnDashCanceled(InputAction.CallbackContext context)
        {
            isDashRequested = false;
        }

        public void Tick()
        {
            string speedId = this.GetInstanceID().ToString();

            if (isShortness)
            {
                if (playerController.GetStamina() < playerController.GetMaxStamina())
                {
                    playerController.HealStamina(Time.deltaTime);
                    OnCurrentStamina?.Invoke(playerController.GetStamina());
                }
                else
                {
                    playerController.CompleateSpeedData(speedId);
                    isShortness = false;
                }
                return;
            }

            if (isDashing)
            {
                if (!isDashRequested || !playerController.IsMove)
                {
                    StopDash(speedId);
                    return;
                }

                if (playerController.GetStamina() > 0)
                {
                    playerController.DownStamina(Time.deltaTime);
                    OnCurrentStamina?.Invoke(playerController.GetStamina());
                }
                else
                {
                    StopDash(speedId);
                    isShortness = true;
                    playerController.DownSpeed(speedId, speedVal);
                }
                return;
            }

            if (isDashRequested && playerController.IsMove && !playerController.IsDebaff())
            {
                isDashing = true;
                playerController.AddSpeed(speedId, speedVal);
            }
            else if (playerController.GetStamina() < playerController.GetMaxStamina())
            {
                playerController.HealStamina(Time.deltaTime);
                OnCurrentStamina?.Invoke(playerController.GetStamina());
            }
        }

        private void StopDash(string speedId)
        {
            isDashing = false;
            playerController.CompleateSpeedData(speedId);
        }
    }
}