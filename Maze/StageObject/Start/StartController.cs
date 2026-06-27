using UnityEngine;
using static MazeGame.MazeGameConstants;


namespace MazeGame
{
    public class StartController : MonoBehaviour
    {
        [SerializeField] private GameObject parentObject;
        [SerializeField] private SoundData startSound;


        private void OnTriggerEnter(Collider other)
        {
            //“G‚ÆƒvƒŒƒCƒ„[‚Å“®ì‚ğ•Ï‚¦‚é
            if (other.gameObject.layer == LayerMask.NameToLayer(MazeGameConstants.PlayerConstants.Layer))
            {
                PlayerController controller;
                if(other.gameObject.TryGetComponent<PlayerController>(out controller))
                {
                    SoundManager.Instance.RequestSe(startSound, other.gameObject.transform.position);
                    controller.HealMaxHP();
                }


            }
        }
    }
}
