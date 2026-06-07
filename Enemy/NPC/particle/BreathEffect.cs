using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class BreathEffect : MonoBehaviour
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
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var collision = ps.collision;
            if (other.CompareTag(MazeConstants.wallTag) || other.CompareTag(MazeConstants.indestructibleWallTag))
            {
                collision.dampen = 1.0f;
            }

            if (!other.gameObject.CompareTag(PlayerConstants.Tag)) return;
            if (isHit) return;
            isHit = true;
            collision.dampen = 0.0f;
            other.gameObject.GetComponent<PlayerController>().AddDamage(Damage);
        }
    }

}
