using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class ItemAction : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private ItemInventory inventory;
        private InputSystem_Actions actions;


        private void Awake()
        {
            playerController.Initilaze += Initialize;
        }

        private void OnEnable()
        {
            actions.Player.ItemChangeRight.performed += OnItemChangeRight;
            actions.Player.ItemChangeLeft.performed += OnItemChangeLeft;
            actions.Player.ItemUse.performed += OnItemUse;
            actions.Player.ItemThrowAway.performed += OnItemThrowAway;
        }

        private void Initialize()
        {
            actions = playerController.Actions;
        }

        private void OnDisable()
        {
            actions.Player.ItemChangeRight.performed -= OnItemChangeRight;
            actions.Player.ItemChangeLeft.performed -= OnItemChangeLeft;
            actions.Player.ItemUse.performed -= OnItemUse;
            actions.Player.ItemThrowAway.performed -= OnItemThrowAway;
            actions = null;
        }

        private void OnItemChangeRight(InputAction.CallbackContext context)
        {
            inventory.NextItem();
        }


        public void OnItemChangeLeft(InputAction.CallbackContext context)
        {
            inventory.BeforeItem();
        }
        public void OnItemUse(InputAction.CallbackContext context)
        {
            inventory.UseItem();
        }
        public void OnItemThrowAway(InputAction.CallbackContext context)
        {
            inventory.RemoveItem();
        }

    }
}