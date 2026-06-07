using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MazeGame
{
    public class MessageScrollManager : MonoBehaviour
    {
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField] private float slideOffset = 20f;   // 出現時の下からのオフセット
        [SerializeField] private float displayTime = 5.0f;  // 表示し続ける時間
        [SerializeField] private float exitOffset = 30f;    // 消える時にさらに上に進む量
        [SerializeField] private SoundData messageSE;

        private Queue<string> _messageQueue = new Queue<string>();
        private bool _isProcessing = false;

        public void EnqueueMessage(string text)
        {
            _messageQueue.Enqueue(text);
            if (!_isProcessing) StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            _isProcessing = true;
            while (_messageQueue.Count > 0)
            {
                string msgText = _messageQueue.Dequeue();
                GameObject obj = Instantiate(messagePrefab, container);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = msgText;

                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(container);

                // アニメーション開始
                StartCoroutine(AnimateMessageLifeCycle(obj));

                yield return new WaitForSeconds(0.2f);
            }
            _isProcessing = false;
        }

        private IEnumerator AnimateMessageLifeCycle(GameObject obj)
        {
            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            RectTransform rt = obj.GetComponent<RectTransform>();

            SoundManager soundManager = SoundManager.Instance;
            if(soundManager != null)
            {
                soundManager.RequestSe(messageSE,this.transform.position, false);
            }

            // --- 1. フェードイン ＆ スライドアップ ---
            Vector3 finalPos = rt.localPosition;
            Vector3 startPos = finalPos - new Vector3(0, slideOffset, 0);
            float elapsed = 0;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                float curve = 1f - Mathf.Pow(1f - t, 3f);
                cg.alpha = t;
                rt.localPosition = Vector3.Lerp(startPos, finalPos, curve);
                yield return null;
            }
            cg.alpha = 1;
            rt.localPosition = finalPos;

            // --- 2. 表示維持（待機） ---
            yield return new WaitForSeconds(displayTime);

            // --- 3. フェードアウト ＆ さらにスライドアップ ---
            elapsed = 0;
            Vector3 exitPos = finalPos + new Vector3(0, exitOffset, 0);

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                float curve = Mathf.Pow(t, 3f);

                cg.alpha = 1f - t;
                rt.localPosition = Vector3.Lerp(finalPos, exitPos, curve);
                yield return null;
            }

            // --- 4. 削除 ＆ 詰め ---
            Destroy(obj);
        }
    }
}
