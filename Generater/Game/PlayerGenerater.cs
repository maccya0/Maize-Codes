using Cysharp.Threading.Tasks.Triggers;
using System;
using Unity.AI.Navigation;
using UnityEngine;

namespace MazeGame
{
    public class PlayerGenerater : BaseGenerator
    {
        [SerializeField] PlayerController Player;
        [SerializeField] GameObject Start;
        [SerializeField] TargetController Target;
        [SerializeField] ItemInventory ItemInventory;
        [SerializeField] CameraContollor CameraContollor;
        public PlayerGenerater()
        {
        }

        public override void Init()
        {
            base.Init();
            Player.IsPlayerControll = false;
            Player.Init(InputManager.Instance.GetInputAction(),Start);
            Target.Init();
            ItemInventory.Init(Player);
            CameraContollor.Init(InputManager.Instance.GetInputAction());
        }

        public override void Generated()
        {
            base.Generated();
            Player.Begin();
            Target.Begin();
            ItemInventory.Begin();
            Player.IsPlayerControll = true;
            InputManager.Instance.ChangeInputModeUIToPlayer();
        }

        public override void Tick()
        {
            base.Tick();
            Player.Tick();
            Target.Tick();
            CameraContollor?.Tick();
        }

        public override void Destroy()
        {
            base.Destroy();
            Target.Destroy();
            CameraContollor.Destroy();
            Player.Destroy();
        }

    }
}
