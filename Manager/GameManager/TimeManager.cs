using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MazeGame
{
    public class TimeManager : BaseManager<TimeManager>
    {
        [SerializeField] private float LimitTime = 60f*5f;
        [SerializeField] private UITimer TimerUI;
        [SerializeField] private float acceleRate = 2.0f;
        private float accele;
        private float erapsedTime;
        private float lastDisplayTime;
        private bool isTimeUp;

        public event Action TimeUpEvent;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            if (TimerUI == null)
            {
                throw new Exception("UITimer is not setting");
            }
            TimeUpEvent += StopGame;
        }

        public override void ManagerStart()
        {
            base.ManagerStart();
            erapsedTime = LimitTime;
            lastDisplayTime = erapsedTime;
            accele = 1f;
            TimerUI.SetTimer(erapsedTime);
            isTimeUp = false;
            StartGame();
        }

        public override void ManagerDestroy()
        {
            base .ManagerDestroy();
            TimeUpEvent -= StopGame;
            // Ç«ÇÒÇ»èÛãµÇ≈Ç‡éûä‘Çí‚é~ÇµÇΩÇ‹Ç‹Ç…ÇµÇ»Ç¢
            StartGame();
        }

        private void Update()
        {
            if(isTimeUp) { return; }
            erapsedTime -= (Time.deltaTime * accele);
            float displayTime = Mathf.CeilToInt(erapsedTime);
            if(lastDisplayTime != displayTime)
            {
                lastDisplayTime = displayTime;
                TimerUI.SetTimer(erapsedTime);
            }
            if (erapsedTime <= 0)
            {
                isTimeUp = true;
                erapsedTime = 0;
                TimerUI.SetTimer(erapsedTime);
                TimeUpEvent?.Invoke();
            }
        }
        public void ResetTimeeAccele()
        {
            accele = 1f;
        }

        public void SetTimeAccele(float _accele)
        {
            accele = _accele;
        }
        public void StopGame()
        {
            Time.timeScale = 0;
        }

        public void StartGame()
        {
            Time.timeScale = 1.0f;
        }
    }
}
