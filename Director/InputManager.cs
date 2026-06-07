using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GameObject ConfigWindow;
        private InputSystem_Actions inputActions;


        public void GenerateInputSysytem()
        {
            if(inputActions != null) return;
            inputActions = new InputSystem_Actions();
            inputActions.Player.ToUI.performed += OnChangeUI;
            inputActions.UI.ToPlayer.performed += OnChangePlayer;

            inputActions.Player.Enable();
        }

        private void OnDestroy()
        {
            inputActions.Player.ToUI.performed -= OnChangeUI;
            inputActions.UI.ToPlayer.performed -= OnChangePlayer;
            inputActions.Dispose();
            inputActions = null;

        }

        public void OnChangeUI(InputAction.CallbackContext context)
        {
            ChangeInputModePlayerToUI();
            UIWindowManager.Instance.ActiveConfigWindow();
        }
        public void OnChangePlayer(InputAction.CallbackContext context)
        {
            ChangeInputModeUIToPlayer();
            UIWindowManager.Instance.ActiveGameUI();
        }

        public InputSystem_Actions GetInputAction()
        {
            return inputActions;
        }

        public void ChangeInputModePlayerToUI()
        {
            inputActions.Player.Disable();
            inputActions.UI.Enable();
        }

        public void ChangeInputModeUIToPlayer()
        {
            inputActions.UI.Disable();
            inputActions.Player.Enable();
        }
    }
}
