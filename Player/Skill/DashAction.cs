using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class DashAction : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float speedVal = 0.25f;
        private InputSystem_Actions actions;
        private Coroutine coroutine;
        private Coroutine healCoroutine;
        private bool enableFlg;
        private bool isShortness;
        public event Action<float> OnCurrentStamina;


        private void Awake()
        {
            playerController.Initilaze += Initialize;
        }

        private void OnEnable()
        {
            actions.Player.Dash.started += OnDashStarted;
            actions.Player.Dash.performed += OnDashPerformed;
            actions.Player.Dash.canceled += OnDashCanceled;
        }

        private void Initialize()
        {
            actions = playerController.Actions;
            enableFlg = true;
            isShortness = false;
            coroutine = null;
            healCoroutine = null;
            playerController.Initilaze -= Initialize;
        }

        private void OnDisable()
        {
            actions.Player.Dash.started -= OnDashStarted;
            actions.Player.Dash.performed -= OnDashPerformed;
            actions.Player.Dash.canceled -= OnDashCanceled;
            actions = null;
        }

        private void OnDashStarted(InputAction.CallbackContext context)
        {
            // スタミナ切れ,デバフ中,移動無しの場合は不許可
            if (playerController.IsDebaff() || isShortness || !playerController.IsMove)
            {
                enableFlg = false;
            }
            else
            {
                enableFlg = true;
                playerController.AddSpeed(this.GetInstanceID().ToString(),speedVal);
                // スタミナ回復中なら止める
                if(healCoroutine != null)
                {
                    StopCoroutine(healCoroutine);
                }
            }
        }


        public void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (!enableFlg) return;
            if (!playerController.IsMove) return;
            // 念のため、古いコルーチンが残っていたら止めてから新しく起動する 
            if (coroutine != null) StopCoroutine(coroutine);
            //ダッシュ許可時のみ走る
            coroutine = StartCoroutine(DashRoutine());
        }

        private IEnumerator DashRoutine()
        {
            //ダッシュ中は徐々に減っていく
            while (playerController.GetStamina() > 0)
            {
                playerController.DownStamina(Time.deltaTime);
                OnCurrentStamina?.Invoke(playerController.GetStamina());
                yield return null;
            }

            // スタミナが回復仕切るまで逆に遅くなる(息切れ)
            isShortness = true;
            playerController.CompleateSpeedData(this.GetInstanceID().ToString());
            playerController.DownSpeed(this.GetInstanceID().ToString(),speedVal);
            float maxStamina = playerController.GetMaxStamina();
            while (playerController.GetStamina() < playerController.GetMaxStamina())
            {
                playerController.HealStamina(Time.deltaTime);
                OnCurrentStamina?.Invoke(playerController.GetStamina());
                yield return null;
            }
            playerController.CompleateSpeedData(this.GetInstanceID().ToString());
            enableFlg = false;
            isShortness = false;
        }

        private void OnDashCanceled(InputAction.CallbackContext context)
        {
            // デバフ中もしくは息切れの時
            if (!enableFlg || isShortness) return;
            // スタミナを使い切らない場合のみ止める
            StopCoroutine(coroutine);
            coroutine = null;
            playerController.CompleateSpeedData(this.GetInstanceID().ToString());
            enableFlg = false;
            // スタミナ回復
            healCoroutine = StartCoroutine(HealRoutine());
        }

        private IEnumerator HealRoutine()
        {
            while (playerController.GetStamina() < playerController.GetMaxStamina())
            {
                playerController.HealStamina(Time.deltaTime);
                OnCurrentStamina?.Invoke(playerController.GetStamina());
                yield return null;
            }
        }
    }
}