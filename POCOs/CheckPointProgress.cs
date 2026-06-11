using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class CheckPointProgress : IDisposable
    {
        private readonly CheckPointController[] checkPointList;
        private int remainCheckpoints;
        private Dictionary<CheckPointController, bool> checkPointDic = new Dictionary<CheckPointController, bool>();
        public Action CheckedAllPoints;

        public CheckPointProgress(CheckPointController[] checkPoints)
        {
            checkPointList = checkPoints;
            foreach (CheckPointController point in checkPointList)
            {
                checkPointDic.Add(point, false);
                point.OnCheckPointReached += UpdateCheckPoint;
            }
        }

        public void Start()
        {
            remainCheckpoints = checkPointList.Length;
            foreach (CheckPointController point in checkPointList)
            {
                checkPointDic[point] = false;
            }
        }

        public void Dispose()
        {
            foreach (CheckPointController point in checkPointList)
            {
                point.OnCheckPointReached -= UpdateCheckPoint;
            }
        }

        private void UpdateCheckPoint(CheckPointController point)
        {
            if (checkPointDic[point]) return;
            checkPointDic[point] = true;
            remainCheckpoints--;
            if(remainCheckpoints == 0)
            {
                CheckedAllPoints?.Invoke();
            }
        }
    }

}
