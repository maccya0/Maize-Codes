using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class BreathEffect : MonoBehaviour
    {
        [SerializeField] private short Damage = 400;
        [SerializeField] private SoundData effectSE;
        private bool isHit;
        private float StickVal = 1.0f;
        private float ThroughVal = 0.0f;

        private void Awake()
        {
            isHit = false;
            SoundManager.Instance.RequestSe(effectSE, this.transform.position);
        }

        void OnParticleCollision(GameObject other)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var collision = ps.collision;
            if (other.CompareTag(MazeGameConstants.MazeConstants.wallTag) || other.CompareTag(MazeGameConstants.MazeConstants.indestructibleWallTag))
            {
                collision.dampen = StickVal;
            }

            if (!other.gameObject.CompareTag(MazeGameConstants.PlayerConstants.Tag)) return;
            if (isHit) return;
            isHit = true;
            collision.dampen = ThroughVal;
            other.gameObject.GetComponent<PlayerController>().AddDamage(Damage);
        }
    }

}
