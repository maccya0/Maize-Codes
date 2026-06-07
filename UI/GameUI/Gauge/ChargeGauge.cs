using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ChargeGauge : MonoBehaviour
{
    [SerializeField] Image chargeGauge;
    [SerializeField] float fadeTime = 0.5f;

    private Coroutine fadeCoroutine; // 重複実行防止用


    private void Awake()
    {
        chargeGauge.fillAmount = 0.0f;
    }

    public void SetGauge(float rate)
    {
        // 0.0〜1.0の間であることを保証
        rate = Mathf.Clamp01(rate);

        chargeGauge.fillAmount = rate;
        CheckAmount();
    }

    private void CheckAmount()
    {
        if (chargeGauge.fillAmount <= 0.0f || chargeGauge.fillAmount >= 1.0f)
        {
            // すでにフェード中なら二重に開始しない
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeOutCoroutine());
            }
        }
        else
        {
            // ゲージが動いている間は表示
            StopFade(); // もしフェード中に値が変わったらフェードを止める
            this.gameObject.SetActive(true);
            SetAlpha(1.0f); // 透明度を戻す
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        Color startColor = chargeGauge.color;
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime / fadeTime;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0.0f);
        this.gameObject.SetActive(false);
        fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = chargeGauge.color;
        c.a = alpha;
        chargeGauge.color = c;
    }

    private void StopFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}