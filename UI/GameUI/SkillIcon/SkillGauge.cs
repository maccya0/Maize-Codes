using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace MazeGame
{
    public class SkillGauge : MonoBehaviour
    {
        [SerializeField] protected GameObject player;
        [SerializeField] private Image skillImage;
        [SerializeField] public TMPro.TextMeshProUGUI textMeshPro;


        private float duration = 0.5f;

        private void Awake()
        {
            if(player == null)
            {
                throw new InvalidOperationException("プレイヤーが未設定");
            }
            if(skillImage == null)
            {
                throw new InvalidOperationException("スキルアイコンが未設定");
            }
        }


        protected void SetGauge(float targetRate)
        {
            DG.Tweening.Sequence mySequence = DOTween.Sequence();
            mySequence.Append(skillImage.DOFillAmount(targetRate, duration));
            mySequence.Play();
        }

        protected void SetCount(int count)
        {
            if(count < 0 )
            {
                count = 0;
            }
            textMeshPro.text = count.ToString();

        }


    }

}
