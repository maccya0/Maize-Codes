using System;
using UnityEngine;

namespace MazeGame
{
    public class BreathAttackController : MonoBehaviour
    {
        [SerializeField] private GameObject BreathEffect;
        [SerializeField] private Transform HeadPos;
        [SerializeField] private SoundData voiceSound;

        void BreathAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }

        private void Awake()
        {
            if (BreathEffect == null)
            {
                throw new Exception("ƒuƒŒƒX‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            }
            if(HeadPos == null)
            {
                throw new Exception("“ª•”‚ªİ’è‚³‚ê‚Ä‚¢‚È‚¢");
            }
        }


        public void EnableBreathEffect()
        {
            Instantiate(BreathEffect,HeadPos);
        }

    }

}
