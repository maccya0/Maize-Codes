using UnityEngine;
using TMPro;

namespace MazeGame
{
    public class ControlsMenuTextSetter : MonoBehaviour
    {
        [System.Serializable]
        public struct ActionTextPair
        {
            public TMP_Text textUI;     // 反映先のTextMeshPro
            [Tooltip("操作説明の日本語名（例：プレイヤー移動、回復スキル など）")]
            public string description;
            [Tooltip("キーボード操作のテキスト（例：W, A, S, D や Q）")]
            public string keyboardKey;
            [Tooltip("ゲームパッド操作のテキスト（例：Left Stick や Y Button）")]
            public string gamepadButton;
        }

        [Header("ーー プレイヤー操作（手動設定） ーー")]
        [SerializeField] private ActionTextPair moveText;
        [SerializeField] private ActionTextPair lookText;
        [SerializeField] private ActionTextPair healText;
        [SerializeField] private ActionTextPair transparentText;
        [SerializeField] private ActionTextPair destroyText;
        [SerializeField] private ActionTextPair dashText;
        [SerializeField] private ActionTextPair slipThroughText;
        [SerializeField] private ActionTextPair selectWarpText;
        [SerializeField] private ActionTextPair allPurposeText;
        [SerializeField] private ActionTextPair itemChangeRightText;
        [SerializeField] private ActionTextPair itemChangeLeftText;
        [SerializeField] private ActionTextPair itemUseText;
        [SerializeField] private ActionTextPair itemThrowAwayText;
        [SerializeField] private ActionTextPair toUIText;

        private void Start()
        {
            RefreshControlsMenu();
        }

        /// <summary>
        /// インスペクターで設定した文字を整形してUIに反映します
        /// </summary>
        public void RefreshControlsMenu()
        {
            SetText(moveText);
            SetText(lookText);
            SetText(healText);
            SetText(transparentText);
            SetText(destroyText);
            SetText(dashText);
            SetText(slipThroughText);
            SetText(selectWarpText);
            SetText(allPurposeText);
            SetText(itemChangeRightText);
            SetText(itemChangeLeftText);
            SetText(itemUseText);
            SetText(itemThrowAwayText);
            SetText(toUIText);
        }

        /// <summary>
        /// <pos>タグを使って位置をカチッと揃えて流し込みます
        /// </summary>
        private void SetText(ActionTextPair pair)
        {
            if (pair.textUI == null) return;

            // 未入力時のセーフティ
            string kb = string.IsNullOrEmpty(pair.keyboardKey) ? "---" : pair.keyboardKey;
            string gp = string.IsNullOrEmpty(pair.gamepadButton) ? "---" : pair.gamepadButton;

            // <pos>タグを使って、項目名・キーボード・パッドの開始位置を完璧に揃える
            // ※35% と 70% の位置から文字がスタートします
            pair.textUI.text = $"{pair.description}<pos=35%>{kb}<pos=70%>{gp}";
        }
    }
}