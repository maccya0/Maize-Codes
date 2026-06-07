using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class ProgressManager : MonoBehaviour
    {
        [SerializeField] private CheckPointController[] checkPointList;
        private int remainCheckpoints;
        private Dictionary<CheckPointController, bool> checkPoints = new Dictionary<CheckPointController, bool>();
        public Action CheckedAllPoints;

        private void Start()
        {
            foreach (CheckPointController point in checkPointList)
            {
                checkPoints.Add(point, false);
                point.OnCheckPointReached += UpdateCheckPoint;
            }
            remainCheckpoints = checkPointList.Length;
        }

        private void OnDestroy()
        {
            foreach (CheckPointController point in checkPointList)
            {
                point.OnCheckPointReached -= UpdateCheckPoint;
            }
        }

        public void InitState()
        {
            foreach (CheckPointController key in checkPoints.Keys)
            {
                checkPoints[key] = false;
            }
            remainCheckpoints = checkPointList.Length;

        }


        private void UpdateCheckPoint(CheckPointController point)
        {
            if (checkPoints[point]) return;
            checkPoints[point] = true;
            remainCheckpoints--;
            if(remainCheckpoints ==0)
            {
                CheckedAllPoints.Invoke();
            }
        }
    }

}
