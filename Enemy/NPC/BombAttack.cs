using UnityEngine;

namespace MazeGame
{
    public class BombAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bombEffectPrefab;
        [SerializeField] private SoundData voiceSound;

        void BombAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }
        void BombStart()
        {
            // îöî≠ÇÃê∂ê¨
            Instantiate(bombEffectPrefab, transform.position, transform.rotation);
        }

        void BombEnd()
        {
            // ç≈å„Ç…è¡ñ≈Ç≥ÇπÇÈ
            Destroy(gameObject);
        }
    }

}
