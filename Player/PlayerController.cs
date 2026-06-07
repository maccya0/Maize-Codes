using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

namespace MazeGame
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private StatusData statusData;
        [SerializeField] private List<SkillAction> skills;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Rigidbody playerRb;
        [SerializeField] private float invincibleTime = 0.5f;


        private Animator animator;
        private HPStatus hpStatus;
        private StatminaStatus statminaStatus;
        private SpeedStatus speedStatus;
        private SpeedModifire speedModifire;
        private bool isPlayingInvincible;

        public bool IsPlayerControll { get; set; }

        private sbyte DeadLine = -10;
        public InputSystem_Actions Actions { get; private set; }
        private Vector2 moveInput;
        public bool IsMove { get; private set; }


        public event Action DiedEvent;
        public event Action<short> HPEvent;
        public event Action<float> StaminaEvent;
        public event Action<float> SpeedEvent;
        public event Action Initilaze;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                throw new InvalidOperationException("Animatorが未設定");
            }
            hpStatus = new HPStatus(statusData.MAXHP);
            statminaStatus = new StatminaStatus(statusData.MAXSTAMINA);
            speedModifire = new SpeedModifire();
            speedStatus = new SpeedStatus(statusData.INITIALSPEED,statusData.MINSPEED, statusData.MAXSPEED,speedModifire);
            IsMove = false;
            isPlayingInvincible = false;
            IsPlayerControll = false;
            inputManager.GenerateInputSysytem();
            Actions = inputManager.GetInputAction();
            Actions.Player.Move.performed += OnMovePerformed;
            Actions.Player.Move.canceled += OnMoveCanceled;
            Actions.Enable();
        }

        private void Start()
        {
            Initilaze?.Invoke();
            foreach (var skill in skills)
            {
                skill.enabled = true;
            }
            gameObject.GetComponent<DashAction>().enabled = true;
            gameObject.GetComponent<ItemAction>().enabled = true;
            StartCoroutine(Respone());
            IsPlayerControll = true;
        }
        private void OnDestroy()
        {
            if (Actions != null)
            {
                Actions.Player.Move.performed -= OnMovePerformed;
                Actions.Player.Move.canceled -= OnMoveCanceled;
                Actions.Disable();
                Actions.Dispose();
            }
            speedModifire.Dispose();
        }


        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMove = true;
        }
        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
            IsMove = false;
        }

        private void FixedUpdate()
        {

            if (hpStatus.GetAlive())
            {
                if(IsPlayerControll)
                {
                    PlayerMoveUpdate();
                }
            }
            else
            {
                DiedEvent.Invoke();
                StartCoroutine(Respone());                
            }

        }

        public IEnumerator InvincibleTimer(float duration)
        {
            isPlayingInvincible = true;
            yield return new WaitForSeconds(duration);
            isPlayingInvincible =false;
        }

        private IEnumerator Respone()
        {
            Maze maze = Maze.Instance;

            while (maze.startTransform == null)
            {
                yield return null;
            }
            this.transform.position = maze.startTransform.position;
            hpStatus.Resopone();
            speedStatus.Resopone();
            speedModifire.ClearModifire();
            statminaStatus.Resopone();
        }

        private void PlayerMoveUpdate()
        {
            //アクション中なら更新しない
            //Y座標が基準値以下になったらリスポーン
            if (transform.position.y < DeadLine)
            {
                DiedEvent.Invoke();
                Respone();
                return;

            }
            // アニメーション設定
            //animator.SetFloat("Speed", moveInput.y);
            //animator.SetFloat("Direction", moveInput.x);
            //animator.SetBool("Jump", false);

            Vector3 moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);

            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }

            Vector3 moveVelocity = new Vector3(moveDirection.x * speedStatus.GetSpeed(), playerRb.linearVelocity.y, moveDirection.z * speedStatus.GetSpeed());
            playerRb.linearVelocity = moveVelocity;
        }

        // Status関連
        public void AddDamage(short damage , bool isInvincibleSet = true)
        {
            if(!hpStatus.GetAlive()) return;
            // 無敵中かつ攻撃が無敵の防御対象の時ははじく
            if (isInvincibleSet && isPlayingInvincible) return;
            hpStatus.SetHp(HPCalcuratar.AddDamage(hpStatus.GetHp(),damage));
            HPEvent?.Invoke(hpStatus.GetHp());
            if(isInvincibleSet)
            {
                StartCoroutine(InvincibleTimer(invincibleTime));
            }
        }

        public void HealMaxHP()
        {
            if (!hpStatus.GetAlive()) return;
            hpStatus.SetHp(hpStatus.GetMaxHp());
            HPEvent?.Invoke(hpStatus.GetHp());
        }
        public void HealHP(short healVal)
        {
            if (!hpStatus.GetAlive()) return;
            hpStatus.SetHp(HPCalcuratar.HealHp(
                hpStatus.GetHp(), 
                healVal, 
                hpStatus.GetMaxHp()
                ));
            HPEvent?.Invoke(hpStatus.GetHp());
        }

        public short GetMaxHP()
        {
            return hpStatus.GetMaxHp();
        }

        public void AddSpeed(string id ,float addSpeed)
        {
            if (!hpStatus.GetAlive()) return;
            speedModifire.RegisterData(id, addSpeed);
            SpeedEvent?.Invoke(speedStatus.GetSpeed());
        }
        public void DownSpeed(string id ,float downSpeed)
        {
            if (!hpStatus.GetAlive()) return;
            speedModifire.RegisterData(id, downSpeed);
            SpeedEvent?.Invoke(speedStatus.GetSpeed());
        }

        public void CompleateSpeedData(string id)
        {
            if (!hpStatus.GetAlive()) return;
            speedModifire.RemoveData(id);
            SpeedEvent?.Invoke(speedStatus.GetSpeed());
        }

        public bool IsDebaff()
        {
            return speedStatus.IsDebaff();
        }

        public float GetSpped()
        {
            return speedStatus.GetSpeed();
        }


        public void HealStamina(float val)
        {
            if (!hpStatus.GetAlive()) return;
            statminaStatus.SetStamina(StaminaCalcurater.AddStamina(val,statminaStatus.GetStamina()));
            StaminaEvent?.Invoke(statminaStatus.GetStamina());
        }

        public void DownStamina(float val)
        {
            if (!hpStatus.GetAlive()) return;
            statminaStatus.SetStamina(StaminaCalcurater.DownStamina(val, statminaStatus.GetStamina()));
            StaminaEvent?.Invoke(statminaStatus.GetStamina());
        }

        public float GetStamina()
        {
            return statminaStatus.GetStamina();
        }

        public float GetMaxStamina()
        {
           return statminaStatus.GetMaxStamina();
        }

        public bool GetAlive()
        {
            return hpStatus.GetAlive();
        }

        public void HealSkill(int num)
        {
            foreach(SkillAction skil in skills)
            {
                skil.UseCharge(num);
            }
        }

    }
}
