using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTweenが必要
using System.Collections;
using System.Collections.Generic;
using MazeGame;

public class ImageTextAnimator : MonoBehaviour
{
    [Header("設定類")]
    [SerializeField] private GameObject charPrefab;    
    [SerializeField] private Transform container;      
    [SerializeField] private float delayPerChar = 0.1f;
    [SerializeField] private float animDuration = 0.5f;

    [Header("アニメーション開始時のオフセット")]
    [SerializeField] private float startScale = 3f;    
    [SerializeField] private float startZOffset = -500f;

    [SerializeField] private List<Sprite> clearSprites = new List<Sprite>();

    [SerializeField] private List<Sprite> faildSprites = new List<Sprite>();
    [SerializeField] private SoundData clearSound;
    [SerializeField] private SoundData failedSound;



    public IEnumerator PlayClearAnimation()
    {
        // 1. 既存のオブジェクトを即座に削除（LayoutRebuilderのために即時性が重要）
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        // Destroyはフレーム最後に行われるため、生成前にリストをクリアした状態を強制する
        yield return null;

        List<CanvasGroup> charGroups = new List<CanvasGroup>();

        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(clearSound, this.transform.position, false);
        }

        // 2. 演出前に「すべての文字」を先に生成して透明で配置する
        foreach (Sprite sprite in clearSprites)
        {
            if (sprite == null) continue;

            GameObject newChar = Instantiate(charPrefab, container);
            newChar.GetComponent<Image>().sprite = sprite;
            CanvasGroup cg = newChar.GetComponent<CanvasGroup>();

            // 最初は完全に透明にしておく（これで場所だけ確保される）
            cg.alpha = 0f;
            charGroups.Add(cg);
        }

        // 3. レイアウトグループの計算をこのフレームで確定させる
        LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());

        // 4. 確保された位置で一文字ずつ演出を開始する
        foreach (var cg in charGroups)
        {
            GameObject obj = cg.gameObject;

            // 初期状態の設定（演出開始直前にセット）
            obj.transform.localScale = Vector3.one * startScale;
            Vector3 targetLocalPos = obj.transform.localPosition;
            obj.transform.localPosition = new Vector3(targetLocalPos.x, targetLocalPos.y, startZOffset);

            // アニメーション
            Sequence seq = DOTween.Sequence();
            seq.SetUpdate(true);
            seq.Join(cg.DOFade(1f, animDuration));
            seq.Join(obj.transform.DOScale(1f, animDuration).SetEase(Ease.OutQuad));
            seq.Join(obj.transform.DOLocalMoveZ(0f, animDuration).SetEase(Ease.OutQuad));

            yield return new WaitForSecondsRealtime(delayPerChar);
        }
    }
    public IEnumerator PlayFinishAnimation()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Destroyはフレーム最後に行われるため、生成前にリストをクリアした状態を強制する
        yield return null;

        List<CanvasGroup> charGroups = new List<CanvasGroup>();

        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(failedSound, this.transform.position, false);
        }


        // 2. 演出前に「すべての文字」を先に生成して透明で配置する
        foreach (Sprite sprite in faildSprites)
        {
            if (sprite == null) continue;

            GameObject newChar = Instantiate(charPrefab, container);
            newChar.GetComponent<Image>().sprite = sprite;
            CanvasGroup cg = newChar.GetComponent<CanvasGroup>();

            // 最初は完全に透明にしておく（これで場所だけ確保される）
            cg.alpha = 0f;
            charGroups.Add(cg);
        }

        // 3. レイアウトグループの計算をこのフレームで確定させる
        LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());


        foreach (var cg in charGroups)
        {
            GameObject obj = cg.gameObject;

            // 初期状態の設定（演出開始直前にセット）
            obj.transform.localScale = Vector3.one;
            Vector3 targetLocalPos = obj.transform.localPosition;
            obj.transform.localPosition = new Vector3(targetLocalPos.x, targetLocalPos.y, 0f);

            // アニメーション
            Sequence seq = DOTween.Sequence();
            seq.SetUpdate(true);
            seq.Join(cg.DOFade(1f, animDuration));
            seq.Join(obj.transform.DOScale(1f, animDuration).SetEase(Ease.OutQuad));

            yield return new WaitForSecondsRealtime(delayPerChar);
        }
    }
}