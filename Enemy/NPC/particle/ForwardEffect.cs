using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class ForwardEffect : MonoBehaviour
    {
        [SerializeField] private short Damage = 400;
        [SerializeField] private SoundData effectSE;
        private bool isHit;

        private void Awake()
        {
            isHit = false;
            SoundManager.Instance.RequestSe(effectSE, this.transform.position);
        }

        void OnParticleCollision(GameObject other)
        {
            if (!other.gameObject.CompareTag(MazeGameConstants.PlayerConstants.Tag)) return;
            if (isHit) return;
            isHit = true;
            other.gameObject.GetComponent<PlayerController>().AddDamage(Damage, false);
        }

    }
}
