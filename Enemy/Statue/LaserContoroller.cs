using UnityEngine;
using System.Collections;
using System;
using MazeGame;

public class LaserContoroller : MonoBehaviour
{
    [SerializeField] private GameObject Effect; //ヒットエフェクト
    [SerializeField] private float _DelayTime=0f; //遅延時間
    [SerializeField] private SoundData laserSound;
    public void Init()
    {
        if (Effect == null)
        {
            throw new InvalidOperationException("プレハブが未設定");
        }
        if (laserSound == null)
        {
            throw new InvalidOperationException("サウンドが未設定");
        }

        //起動時に必要な初期化を行う
        Effect.gameObject.SetActive(false);
    }
    public void Lasar()
    {
        // 当たり判定管理のコルーチン
        StartCoroutine(LaserCoroutine());
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(laserSound, this.transform.position);
        }
        float effectTime = Effect.GetComponent<ParticleSystem>().main.duration + _DelayTime;
        Destroy(gameObject, effectTime);
    }

    private IEnumerator LaserCoroutine()
    {
        var delayCount = Mathf.Max(0, _DelayTime);
        yield return new WaitForSeconds(delayCount);
        Effect.gameObject.SetActive(true);

    }
}
