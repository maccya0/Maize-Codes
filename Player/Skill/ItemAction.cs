using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class ItemAction : MonoBehaviour
    {
        [SerializeField] private ItemInventory inventory;
        private InputSystem_Actions actions;
        private PlayerController playerController;


        public void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            this.playerController = _playerController;
            this.actions = _actions;
            actions.Player.ItemChangeRight.performed += OnItemChangeRight;
            actions.Player.ItemChangeLeft.performed += OnItemChangeLeft;
            actions.Player.ItemUse.performed += OnItemUse;
            actions.Player.ItemThrowAway.performed += OnItemThrowAway;
        }

        public void Begin()
        {
        }
        public void Destroy()
        {
            actions.Player.ItemChangeRight.performed -= OnItemChangeRight;
            actions.Player.ItemChangeLeft.performed -= OnItemChangeLeft;
            actions.Player.ItemUse.performed -= OnItemUse;
            actions.Player.ItemThrowAway.performed -= OnItemThrowAway;
        }

        private void OnItemChangeRight(InputAction.CallbackContext context)
        {
            if (!playerController.IsPlayerControll) return;
            inventory.NextItem();
        }


        public void OnItemChangeLeft(InputAction.CallbackContext context)
        {
            if (!playerController.IsPlayerControll) return;
            inventory.BeforeItem();
        }
        public void OnItemUse(InputAction.CallbackContext context)
        {
            if (!playerController.IsPlayerControll) return;
            inventory.UseItem();
        }
        public void OnItemThrowAway(InputAction.CallbackContext context)
        {
            if (!playerController.IsPlayerControll) return;
            inventory.RemoveItem();
        }

    }
}