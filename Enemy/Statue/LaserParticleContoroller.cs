using UnityEngine;
using MazeGame;

public class LaserParticleContoroller : MonoBehaviour
{
    [SerializeField] private short Damage = 200; //ダメージ
    [SerializeField] private float Impact = 10.0f;
    private bool isHit = false;

    void OnParticleCollision(GameObject other)
    {
        if (isHit) return;
        if(other.tag != MazeGameConstants.PlayerConstants.Tag) return;

        // パーティクルが当たった時の処理
        isHit = true;
        PlayerController controller = other.GetComponent<PlayerController>();
        Rigidbody body = other.GetComponent<Rigidbody>();
        Vector3 direct = -other.transform.forward;
        body.AddForce(direct* Impact, ForceMode.Impulse);
        controller.AddDamage(Damage);
        //ヒットエフェクトや効果音の設定
    }
}
