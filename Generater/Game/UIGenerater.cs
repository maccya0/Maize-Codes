using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class UIGenerater : BaseGenerator
    {
        [SerializeField] GameConfigWindowController GameConfigWindowController;
        [SerializeField] GameExplainWindowController GameExplainWindowController;
        [SerializeField] GameLevelWindowController GameLevelWindowController;
        [SerializeField] GameResultWindowController GameResultWindowController;
        [SerializeField] ImageTextAnimator ImageTextAnimator;
        LevelSelection LevelSelection;
        public UIGenerater(LevelSelection _levelSelection)
        {
            LevelSelection = _levelSelection;
        }

        public override void Init()
        {
            base.Init();
            GameConfigWindowController.WindowInit();
            GameExplainWindowController.WindowInit();
            GameLevelWindowController.WindowInit(LevelSelection);
            GameResultWindowController.WindowInit();
            MessageScrollManager.Instance.ManagerInit();
        }

        public override void Generated()
        {
            // 特になし
            MessageScrollManager.Instance.ManagerStart();
        }

        public override void Tick()
        {
            // 特になし

        }
        public override void Destroy()
        {
            // 特になし
            MessageScrollManager.Instance.ManagerDestroy();
        }

        public void EndGame(bool judge )
        {
            base.EndGame();
            GameResultWindowController.StartCoroutine(LaunchResultSequence(judge, ImageTextAnimator));
        }
        private System.Collections.IEnumerator LaunchResultSequence(bool judge, ImageTextAnimator animator)
        {
            if (judge)
            {
                // クリア文字の演出終了を待つ
                yield return GameResultWindowController.StartCoroutine(animator.PlayClearAnimation());
            }
            else
            {
                // 失敗文字の演出終了を待つ
                yield return GameResultWindowController.StartCoroutine(animator.PlayFinishAnimation());
            }

            // 演出が完全に終わったら、満を持してリザルトウィンドウを立ち上げる！
            ShowResultUI();
        }

        private void ShowResultUI()
        {
            // 入力モードをUI用に切り替え
            InputManager.Instance.ChangeInputModePlayerToUI();
            // リザルトウィンドウをアクティブにする
            UIWindowManager.Instance.ActiveResultWindow();
        }
    }
}
