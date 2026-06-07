using UnityEngine;

namespace MazeGame
{
    public class AreaAttack : MonoBehaviour
    {
        [SerializeField] private GameObject AreaEffectPrefab;
        [SerializeField] private SoundData voiceSound;

        void AreaAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }

        void AreaGenerate()
        {
            Instantiate(AreaEffectPrefab, this.transform.position, this.transform.rotation);
        }
    }

}
