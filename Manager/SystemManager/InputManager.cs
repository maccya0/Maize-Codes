using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class InputManager : BaseManager<InputManager>
    {
        [SerializeField] GameObject ConfigWindow;
        private InputSystem_Actions inputActions;


        protected override void Awake()
        {
            base.Awake();
            if(Instance != this) return;
            if (inputActions != null) return;
            inputActions = new InputSystem_Actions();
            inputActions.Player.ToUI.performed += OnChangeUI;
            inputActions.UI.ToPlayer.performed += OnChangePlayer;
        }

        public override void ManagerStart()
        {
            base.ManagerStart();
            ChangeInputModeUIToPlayer();
        }

        public override void ManagerDestroy()
        {
            if (inputActions == null) return;
            base.ManagerDestroy();
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
