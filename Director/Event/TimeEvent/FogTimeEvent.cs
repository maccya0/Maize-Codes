
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class FogTimeEvent : MazeTimeEvent
    {

        [SerializeField] private float maxFogDensity = 0.05f; // 霧の最大濃度
        [SerializeField] private float fadeDuration = 2.0f;   // 霧が濃くなる/薄くなるまでの時間
        protected override void EventStart()
        {
            // Unityのライティング設定でフォグを有効にする
            base.OutputMessage("霧が濃くなってきた");
        }
        protected override void EventEnd()
        {
            // 何もしない
            return;
        }

        protected override IEnumerator EventAction()
        {
            // --- 1. 霧を濃くする (Fade In) ---
            RenderSettings.fog = true;
            float startDensity = RenderSettings.fogDensity;
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, (startDensity + maxFogDensity), elapsed / fadeDuration);
                yield return null;
            }

            // --- 2. メインの待機時間 ---
            // base.EventTime から、フェードにかかる前後2回分の時間を引く
            float waitTime = Mathf.Max(0, base.EventTime - (fadeDuration * 2));
            yield return new WaitForSeconds(waitTime);

            base.OutputMessage("霧が薄くなった");

            // --- 3. 霧を薄くする (Fade Out) ---
            elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, (startDensity + maxFogDensity), elapsed / fadeDuration);
                yield return null;
            }
        }
    }
}