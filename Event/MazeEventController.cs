using UnityEngine;
using System.Collections.Generic;
using System;

namespace MazeGame
{
    public class MazeEventController : MonoBehaviour
    {
        [Header("--------------イベント設定--------------")]
        [SerializeField] private MazeTimeEvent[] randamTimeEvents;
        [SerializeField] private MazeOnceEvent[] randamOnceEvents;
        [SerializeField] private float EventInterval = 30.0f;

        private int onceEventTotal;
        private int timeEventTotal;
        private float elapsedTime;
        private bool isRunning;
        private int BoolRange = 2;

        public void Init()
        {
            elapsedTime = 0f;
            isRunning = false;

            timeEventTotal = 0;
            if (randamTimeEvents != null)
            {
                foreach (var e in randamTimeEvents) if (e != null) timeEventTotal += e.rateVal;
            }

            onceEventTotal = 0;
            if (randamOnceEvents != null)
            {
                foreach (var e in randamOnceEvents) if (e != null) onceEventTotal += e.rateVal;
            }
        }

        public void StartEvents()
        {
            isRunning = true;
        }

        public void Tick()
        {
            if (!isRunning) return;

            elapsedTime += Time.deltaTime;
            GenerateEvent();
        }

        public void StopEvents()
        {
            isRunning = false;
            foreach (var e in randamTimeEvents)
            {
                if (e != null)
                {
                    e.StopEvent();
                }
            }
        }

        private void GenerateEvent()
        {
            try
            {
                if (elapsedTime <= EventInterval) return;
                elapsedTime = 0;
                bool target = UnityEngine.Random.Range(0, BoolRange) == 1 ? true : false;
                MazeEvent mazeEvent = null;
                if (target)
                {
                    int randamRate = UnityEngine.Random.Range(0, timeEventTotal);
                    int weight = 0;
                    foreach (var timeEvent in randamTimeEvents)
                    {
                        weight += timeEvent.rateVal;
                        if (randamRate < weight)
                        {
                            mazeEvent = timeEvent;
                            break;
                        }
                    }
                }
                else
                {
                    int randamRate = UnityEngine.Random.Range(0, onceEventTotal);
                    int weight = 0;
                    foreach (var onceEvent in randamOnceEvents)
                    {
                        weight += onceEvent.rateVal;
                        if (randamRate < weight)
                        {
                            mazeEvent = onceEvent;
                            break;
                        }
                    }
                }
                if (mazeEvent != null)
                {
                    mazeEvent.TriggerEvent();
                }

            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}