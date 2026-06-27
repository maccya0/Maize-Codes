using System;
using UnityEngine;
using static MazeGame.MazeGameConstants;


namespace MazeGame
{
    public class CheckPointController : MonoBehaviour
    {
        private bool checkedPoint = false;
        public event Action<CheckPointController> OnCheckPointReached;
        [SerializeField] private SoundData checkPointSound;
        [SerializeField] private MessageScrollManager messageScrollManager;
        [SerializeField] private GameObject parentObject;

        private void OnDestroy()
        {
            // ì¡Ç…âΩÇ‡ÇµÇ»Ç¢
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(MazeGameConstants.PlayerConstants.Layer))
            {
                if(!checkedPoint)
                {
                    OnCheckPointReached?.Invoke(this);
                    checkedPoint = true;
                    SoundManager soundManager = SoundManager.Instance;
                    if (soundManager != null)
                    {
                        soundManager.RequestSe(checkPointSound, this.transform.position, false);
                    }
                    messageScrollManager.EnqueueMessage("êÖèªÇ…êGÇÍÇΩ");
                    EnemyManager.Instance.DeleatePatrolPoint(parentObject);
                }
                else
                {
                    messageScrollManager.EnqueueMessage("ä˘Ç…ñKÇÍÇΩêÖèªÇæ");
                }
            }
        }
    }
}
