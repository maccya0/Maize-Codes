using System;
using System.Collections.Generic;


namespace MazeGame
{
    public class SpeedModifire: IDisposable
    {
        private Dictionary<string, float> speedModifiers;
        private float sumData;
        public event Action OnChanged;

        public SpeedModifire()
        {
            speedModifiers = new Dictionary<string, float>();
            sumData = 0f;
        }

        public void Dispose()
        {
            speedModifiers.Clear();
            speedModifiers = null;
        }

        public void RegisterData(string id, float data)
        {
            if (IsRegisterData(id)) return;
            speedModifiers.Add(id, data);
            sumData += data;
            OnChanged.Invoke();
        }

        public void RemoveData(string id)
        {
            if (!speedModifiers.ContainsKey(id)) return; 
            sumData -= speedModifiers[id];
            speedModifiers.Remove(id);
            OnChanged.Invoke();
        }

        public bool IsRegisterData(string id)
        {
            return speedModifiers.ContainsKey(id); 
        }

        public float GetSpeedModifier(string id)
        {
            return speedModifiers[id];
        }

        public void ClearModifire()
        {
            speedModifiers.Clear();
            sumData = 0f;
            OnChanged.Invoke();
        }

        public float GetSumData()
        {
            return sumData;
        }
    }

}
