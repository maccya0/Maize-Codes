using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace MazeGame
{
    public class StaminaGauge : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Image staminaImage;


        private float duration = 0.5f;
        private HPStatus playerStatus;
        private float MaxStamina;
        private void Awake()
        {
            if (player == null)
            {
                throw new InvalidOperationException("プレイヤーが未設定");
            }
            if (staminaImage == null)
            {
                throw new InvalidOperationException("ゲージが未設定");
            }
            player.StaminaEvent += SetGauge;

        }

        private void Start()
        {
            MaxStamina = player.GetMaxStamina();
        }

        private void SetGauge(float newStamina)
        {
            float rate = newStamina / MaxStamina;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(staminaImage.DOFillAmount(rate, duration));
            mySequence.Play();
        }
    }

}
