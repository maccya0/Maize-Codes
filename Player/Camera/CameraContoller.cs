using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class CameraContollor : MonoBehaviour
    {
        /* インスペクター操作 */
        [SerializeField] private GameObject player;            //カメラの追跡対象
        [SerializeField] private float pitchRange = 60f;
        [SerializeField] private float sensitiveMove = 0.5f;
        [SerializeField] private PlayerController playerController;

        /* カメラ操作関連 */
        private InputSystem_Actions actions;
        private Vector2 lookInput;
        private float pitch = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(InputSystem_Actions inputActions)
        {
            actions = inputActions;
            actions.Player.Look.performed += OnLookPerformed;
            actions.Player.Look.canceled += OnLookCanceld;
        }

        public void Destroy()
        {
            actions.Player.Look.performed -= OnLookPerformed;
            actions.Player.Look.canceled -= OnLookCanceld;

        }

        public void OnLookPerformed(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }
        public void OnLookCanceld(InputAction.CallbackContext context)
        {
            lookInput = Vector2.zero;
        }

        public void Tick()
        {
            //キャラクターの移動に合わせたカメラ移動
            MoveCamera();

        }

        private void MoveCamera()
        {
            // プレイヤーを回転させることで子オブジェクトのカメラも動く
            if (Mathf.Abs(lookInput.x) >= 0.4)
            {
                player.transform.Rotate(Vector3.up * lookInput.x * 1.0f);
            }
            else
            {
                player.transform.Rotate(Vector3.up * lookInput.x * 1.0f * sensitiveMove);
            }

            if (Mathf.Abs(lookInput.y) >= 0.4)
            {
                pitch -= lookInput.y * 100.0f * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);
                transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
            else
            {
                pitch -= lookInput.y * 100.0f * sensitiveMove * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);
                transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
        }
    }

}