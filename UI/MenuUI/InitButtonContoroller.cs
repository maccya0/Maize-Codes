using System;
using UnityEngine;

namespace MazeGame
{
    public class InitButtonController : UICursorContoroller
    {
        public Action OnInit;
        [SerializeField] SoundData decideData;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, transform.position, false);
            }

            OnInit?.Invoke();
        }
    }

}
