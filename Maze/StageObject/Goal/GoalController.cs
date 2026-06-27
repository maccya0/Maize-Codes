using System;
using UnityEngine;
using static MazeGame.MazeGameConstants;


namespace MazeGame
{
    public class GoalController : MonoBehaviour
    {
        [SerializeField] private GameObject parentObject;
        public event Action ReachedGoal;
        private bool reach = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(MazeGameConstants.PlayerConstants.Layer))
            {
                reach = true;
                ReachedGoal.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            reach = false;
        }
    }
}
