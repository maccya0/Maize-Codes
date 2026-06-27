using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


namespace MazeGame
{
    public class HealthGauge : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Image healthImage;
        [SerializeField] private Image burnImage;


        private float duration = 0.25f;
        private float derayTime = 0.5f;
        private short MaxHP;

        private void Awake()
        {
            if(player == null)
            {
                throw new InvalidOperationException("プレイヤーが未設定");
            }
            if(healthImage == null || burnImage == null)
            {
                throw new InvalidOperationException("ゲージが未設定");
            }
            player.HPEvent += SetGauge;
        }

        private void Start()
        {
            MaxHP = player.GetMaxHP();

        }

        private void OnDestroy()
        {
            player.HPEvent -= SetGauge;

        }

        private void SetGauge(short newHP)
        {

            // short型にキャストされるので明示的にfloatにキャスト
            float rate = (float)newHP / (float)MaxHP;

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(healthImage.DOFillAmount(rate, duration));
            mySequence.Append(burnImage.DOFillAmount(rate, duration).SetDelay(derayTime));
            mySequence.Play();
        }
    }

}
