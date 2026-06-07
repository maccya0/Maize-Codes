using System;
using UnityEngine;

namespace MazeGame
{
    public class GameTimeManager : MonoBehaviour
    {
        [SerializeField] private float LimitTime = 60f*5f;
        [SerializeField] private UITimer Timer;
        private float erapsedTime;
        private float accele;
        private bool isTimeUp;

        public event Action TimeUpEvent;


        //àÍî‘ëÅÇ≠ìÆçÏÇ≥ÇπÇΩÇ¢ÇÃÇ≈Awake
        private void Awake()
        {
            erapsedTime = LimitTime;
            accele = 1f;
            if (Timer == null)
            {
                throw new Exception("UITimer is not setting");
            }
            TimeUpEvent += StopGame;
            StartGame();
        }

        private void OnDestroy()
        {
            // ì¡Ç…âΩÇ‡ÇµÇ»Ç¢
        }

        public void InitInfo()
        {
            erapsedTime = LimitTime;
            accele = 1f;
            Timer.SetTimer(erapsedTime);
            isTimeUp = false;
            StartGame();
        }

        private void Update()
        {
            if(isTimeUp) { return; }
            erapsedTime -= (Time.deltaTime * accele);
            Timer.SetTimer(erapsedTime);
            if (erapsedTime <= 0)
            {
                isTimeUp = true;
                TimeUpEvent.Invoke();
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
