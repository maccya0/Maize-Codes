using UnityEngine;
using System.Collections;
using MazeGame;
using static MazeGame.MazeGameConstants;
using System;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect; // 爆発エフェクト
    [SerializeField] private SphereCollider _explosionCollider;  // 爆発範囲
    [SerializeField] private short _explosionDamage; // 爆発ダメージ
    [SerializeField] private float _explosionImpact; // 爆発威力
    [SerializeField] private float _explosionDelayTime; // 爆発遅延時間
    [SerializeField] private SoundData bombSound;
    private bool isHit;

    private void Initialize()
    {
        if (_explosionEffect == null)
        {
            throw new InvalidOperationException("プレハブが未設定");
        }
        if (_explosionCollider == null)
        {
            throw new InvalidOperationException("コライダーが未設定");
        }

        //起動時に必要な初期化を行う
        _explosionEffect.gameObject.SetActive(false);
        _explosionCollider.enabled = false;
        //_explosionSound.Stop();
        isHit = false;
    }

    public void Explosion()
    {
        // 初期化
        Initialize();

        // 当たり判定管理のコルーチン
        StartCoroutine(ExplodeCoroutine());

    }

    private IEnumerator ExplodeCoroutine()
    {
        var delayCount = Mathf.Max(0, _explosionDelayTime);
        yield return new WaitForSeconds(delayCount);

        // 時間経過したらコライダを有効化して爆発の当たり判定と共に再生
        _explosionCollider.enabled = true;
        _explosionEffect.gameObject.SetActive(true);
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(bombSound,this.transform.position);
        }
        float effectTime = _explosionEffect.GetComponent<ParticleSystem>().main.duration + _explosionDelayTime;
        Destroy(gameObject, effectTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;
        //プレイヤー以外なら返す
        if (other.tag != PlayerConstants.Tag) return;
        isHit = true;
        // 衝突対象がRigidbodyの配下であるかを調べる
        var rigidBody = other.GetComponent<Rigidbody>();

        // 吹っ飛ばす
        rigidBody.AddExplosionForce(_explosionImpact, this.transform.position, _explosionCollider.radius);

        //ダメージを加える
        other.GetComponentInParent<PlayerController>().AddDamage(_explosionDamage);
    }

}
