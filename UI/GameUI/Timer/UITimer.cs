using TMPro;
using UnityEngine;

namespace MazeGame
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerUI;
        [SerializeField] private SoundData limitSound;
        bool limitFlag;

        private void Awake()
        {
            if(timerUI == null)
            {
                throw new System.Exception("timerUI is not setting");
            }
            limitFlag = false;
        }

        public void SetTimer(float remainTime)
        {
            // ïbÅ®ï™Ç÷ïœçX
            string minute = ((int)remainTime / 60).ToString();
            string second = ((int)remainTime % 60).ToString();
            if ((int)remainTime % 60<=9)
            {
                second = "0" + second;
            }
            string time = minute.ToString() + " : " + second.ToString();
            timerUI.text = time;

            if( !limitFlag && remainTime / 60 <= 1)
            {
                limitFlag = true;
                SoundManager soundManager = SoundManager.Instance;
                if (soundManager != null)
                {
                    soundManager.RequestSe(limitSound,this.transform.position, false);
                }
            }

        }

    }
}
