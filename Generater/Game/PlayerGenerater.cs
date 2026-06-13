using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class PlayerGenerater : BaseGenerator
    {
        [SerializeField] PlayerController Player;
        [SerializeField] GameObject Start;
        [SerializeField] TargetController Target;
        public PlayerGenerater()
        {
        }

        public override void Init()
        {
            base.Init();
            Player.IsPlayerControll = false;
            Player.Init(InputManager.Instance.GetInputAction(),Start);
            Target.Init();
        }

        public override void Generated()
        {
            base.Generated();
            Player.Begin();
            Target.Begin();
            Player.IsPlayerControll = true;
            InputManager.Instance.ChangeInputModeUIToPlayer();
        }

        public override void Tick()
        {
            base.Tick();
            Player.Tick();
            Target.Tick();
        }

        public override void Destroy()
        {
            base.Destroy();
            Player.Destroy();
            Target.Destroy();
        }

    }
}
