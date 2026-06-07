using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/SoundData")]
    public class SoundData : ScriptableObject
    {
        [SerializeField] public AudioClip clip;
        [SerializeField][Range(0f, 1f)]  public float volume = 1.0f;
        [SerializeField][Range(0.5f, 2.0f)] public float pitch = 1.0f;
        [SerializeField] public bool loop = false;
        public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    }
}
