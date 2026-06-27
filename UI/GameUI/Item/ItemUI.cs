using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Newtonsoft.Json.Bson;

namespace MazeGame
{
    public class ItemUI : MonoBehaviour
    {
        [Header("現在の表示（消える側）")]
        [SerializeField] private CanvasGroup currentGroup;
        [SerializeField] private RectTransform currentRect;
        [SerializeField] private Image currentItemImage;
        [SerializeField] private Text currentItemName;

        [Header("新しい表示（現れる側）")]
        [SerializeField] private CanvasGroup newGroup;
        [SerializeField] private RectTransform newRect;
        [SerializeField] private Image newItemImage;
        [SerializeField] private Text newItemName;

        [Header("設定")]
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float slideOffset = 100f; // スライド距離

        public void Init()
        {
        }

        public void Begin()
        {
            newGroup.alpha = 0;

        }

        public void Destroy()
        {
        }

        public void InitItem(ItemBase itemBase)
        {
            //　新しい方は消えておく
            newGroup.alpha = 0;
            // 空のアイテム表示
            currentItemImage.sprite = itemBase.icon;
            currentItemName.text = itemBase.itemName;
            currentRect.anchoredPosition = Vector2.zero;
            currentGroup.alpha = 1;

        }

        public void UpdateItem(ItemBase itemBase)
        {
            // すべてのアニメーションを即座に完了させて重複を防ぐ
            currentRect.DOKill();
            newRect.DOKill();

            // 1. 新しいアイテムの情報をセットして、左側に配置
            newItemImage.sprite = itemBase.icon;
            newItemName.text = itemBase.itemName;
            newRect.anchoredPosition = new Vector2(-slideOffset, 0);
            newGroup.alpha = 0;

            // 2. 現在のアイテムを右へ逃がしながらフェードアウト
            currentRect.DOAnchorPos(new Vector2(slideOffset, 0), duration).SetEase(Ease.OutCubic);
            currentGroup.DOFade(0, duration);

            // 3. 新しいアイテムを中央へスライドさせながらフェードイン
            newRect.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutCubic);
            newGroup.DOFade(1, duration).OnComplete(() =>
            {
                // 4. アニメーション終了時の入れ替え作業
                // Currentの表示を今回のアイテムで上書きし、中央に戻しておく（次回の消える役にするため）
                currentItemImage.sprite = newItemImage.sprite;
                currentItemName.text = newItemName.text;
                currentRect.anchoredPosition = Vector2.zero;
                currentGroup.alpha = 1;

                // New側はまた次に備えて透明にしておく
                newGroup.alpha = 0;
            });
        }
    }
}