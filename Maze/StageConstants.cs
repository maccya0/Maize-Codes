

using System.Numerics;
using UnityEngine;

namespace MazeGame
{
    public class StageConstants : ScriptableObject
    {
        /* 定数パラメータ */
        [SerializeField] public float PosOffset = 1.0f;  // 迷路内オブジェクトの生成位置オフセット
        [SerializeField] public float WallPosOffset = 2.5f;  // 壁のY座標オフセット
        [SerializeField] public float WallScaleYVal = 5.0f;    // 壁のYスケール値
        [SerializeField] public string RootName = "MazeRoot";    // Rootの名前
        [SerializeField] public int GenerateRange = 255;    // ランダム生成範囲
        [SerializeField] public float StageHeightLimit = 5.0f;
        [SerializeField] public float GoalGimicOfsetHeight = 4.5f;
        [SerializeField] public float GoalGimicOfsetVertcal = 0.35f;
        [SerializeField] public float StartGimicOfsetHeight = 4.5f;
        [SerializeField] public float StartGimicOfsetVertcal = 0.35f;
        [SerializeField] public float CheckpointGimicOfsetHeight = 0.35f;
        [SerializeField] public float CheckpointGimicOfsetVertcal = 4.5f;
    }


}
