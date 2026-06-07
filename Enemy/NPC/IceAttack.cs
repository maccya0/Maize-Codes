using Unity.Behavior;
using UnityEngine;

namespace MazeGame
{
    public class IceAttack : MonoBehaviour
    {
        [SerializeField] private GameObject IceEffectPrefab;
        private BehaviorGraphAgent agent;
        [SerializeField] private SoundData voiceSound;

        void IceAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }
        void IceGenerate()
        {
            if(agent == null)
            {
                agent = GetComponent<BehaviorGraphAgent>();

            }
            if (agent != null && agent.BlackboardReference.GetVariable<GameObject>("Target", out BlackboardVariable<GameObject> target))
            {
                GameObject player = target.Value;
                Instantiate(IceEffectPrefab, player.transform.position, player.transform.rotation);
            }
        }
    }

}
