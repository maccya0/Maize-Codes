using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/SkillData")]
    public class SkillData : ScriptableObject
    {
        [SerializeField] public byte actionLimit = 5;
        [SerializeField] public float actionTime = 1.0f;
        [SerializeField] public float recastTime = 1.0f;
        [SerializeField] public GameObject particle;
        [SerializeField] public SoundData sound;
    }

}
