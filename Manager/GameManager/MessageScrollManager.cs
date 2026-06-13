using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    public class MessageScrollManager : BaseManager<MessageScrollManager>
    {
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField] private float slideOffset = 20f;   // 出現時の下からのオフセット
        [SerializeField] private float displayTime = 5.0f;  // 表示し続ける時間
        [SerializeField] private float exitOffset = 30f;    // 消える時にさらに上に進む量
        [SerializeField] private SoundData messageSE;

        private Queue<string> _messageQueue;
        private bool _isProcessing = false;

        public override void ManagerInit()
        {
            base.ManagerInit();
            _messageQueue = new Queue<string>();
            _isProcessing = false;
        }

        public override void ManagerStart()
        {
            base.ManagerStart();
            if (container != null)
            {
                foreach (Transform child in container)
                {
                    Destroy(child.gameObject);
                }
            }

            StopAllCoroutines();
            _messageQueue.Clear();
            _isProcessing = false;
        }

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

                StartCoroutine(AnimateMessageLifeCycle(obj));

                yield return new WaitForSeconds(0.2f);
            }
            _isProcessing = false;
        }

        private IEnumerator AnimateMessageLifeCycle(GameObject obj)
        {
            if (obj == null) yield break;

            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            RectTransform rt = obj.GetComponent<RectTransform>();

            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(messageSE, this.transform.position, false);
            }

            Vector2 basePosition = rt.anchoredPosition;

            // --- 1. フェードイン ＆ スライドアップ ---
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                if (obj == null) yield break;

                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;

                float curve = 1f - Mathf.Pow(1f - t, 3f);

                cg.alpha = t;

                float currentOffset = Mathf.Lerp(slideOffset, 0f, curve);
                rt.anchoredPosition = basePosition - new Vector2(0, currentOffset);

                yield return null;
            }
            cg.alpha = 1;
            rt.anchoredPosition = basePosition;

            yield return new WaitForSeconds(displayTime);

            elapsed = 0;
            while (elapsed < fadeDuration)
            {
                if (obj == null) yield break;

                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                float curve = Mathf.Pow(t, 3f);

                cg.alpha = 1f - t;

                float currentOffset = Mathf.Lerp(0f, exitOffset, curve);
                rt.anchoredPosition = basePosition + new Vector2(0, currentOffset);

                yield return null;
            }

            if (obj != null)
            {
                Destroy(obj);

                yield return null;
                if (container != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(container);
                }
            }
        }
    }
}