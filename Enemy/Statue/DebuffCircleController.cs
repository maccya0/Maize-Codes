using UnityEngine;
using System.Collections;
using MazeGame;
using static MazeGame.MazeGameConstants;
using System;

public class DebuffCircleController : MonoBehaviour
{
    [SerializeField] private GameObject Circle; // 爆発エフェクト
    [SerializeField] private GameObject HitEffect; //ヒットエフェクト
    [SerializeField] private Collider DebuffCollider;  //デバフ範囲
    [SerializeField] private float DebuffDelayTime; //デバフ遅延時間
    [SerializeField] SoundData debuffSound;
    private bool isTrigger;

    void Initialize()
    {

        if (Circle == null || HitEffect == null)
        {
            throw new InvalidOperationException("プレハブが未設定");
        }
        if (debuffSound == null)
        {
            //throw new InvalidOperationException("サウンドが未設定");
        }
        if (DebuffCollider == null)
        {
            throw new InvalidOperationException("コライダーが未設定");
        }
        //起動時に必要な初期化を行う
        Circle.gameObject.SetActive(false);
        DebuffCollider.enabled = false;
        isTrigger = false;

    }

    public void Debuff()
    {
        Initialize();
        // 当たり判定管理のコルーチン
        StartCoroutine(HitCoroutine());
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(debuffSound, this.transform.position);
        }
        float effectTime = Circle.GetComponent<ParticleSystem>().main.duration + DebuffDelayTime;

        Destroy(gameObject, effectTime);
    }

    private IEnumerator HitCoroutine()
    {
        var delayCount = Mathf.Max(0, DebuffDelayTime);
        yield return new WaitForSeconds(delayCount);
        // 時間経過したらコライダを有効化してデバフの当たり判定が出る
        DebuffCollider.enabled = true;
        Circle.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isTrigger) return;
        //プレイヤー以外なら返す
        if (other.tag != PlayerConstants.Tag) return;
        isTrigger = true;
        GameObject hitEffect = Instantiate(HitEffect, other.transform.position, Quaternion.identity);
        hitEffect.GetComponent<SpeedDebuffController>().Debuff(other.gameObject);

    }

}
