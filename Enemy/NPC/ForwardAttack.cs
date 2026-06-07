using Unity.Behavior;
using UnityEngine;

namespace MazeGame
{
    public class ForwardAttack : MonoBehaviour
    {
        [SerializeField] private GameObject ForwardEffectPrefab;
        private Animator animator;
        private BehaviorGraphAgent agent;
        [SerializeField] private SoundData voiceSound;

        void ForwardAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void ForwardGenerate()
        {
            if (agent == null)
            {
                agent = GetComponent<BehaviorGraphAgent>();
            }

            if (agent != null && agent.BlackboardReference.GetVariable<GameObject>("Target", out BlackboardVariable<GameObject> target))
            {
                GameObject player = target.Value;
                if (animator != null && player != null)
                {
                    Transform chest = animator.GetBoneTransform(HumanBodyBones.Chest);
                    Vector3 spawnPos = (chest != null) ? chest.position : this.transform.position + Vector3.up * 1f;

                    spawnPos += this.transform.forward * 0.5f;

                    GameObject effect = Instantiate(ForwardEffectPrefab, spawnPos, Quaternion.identity);

                    Vector3 direction = player.transform.position - effect.transform.position;

                    direction.y = 0;

                    if (direction.sqrMagnitude > 0.001f)
                    {
                        effect.transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
            }
        }
    }
}