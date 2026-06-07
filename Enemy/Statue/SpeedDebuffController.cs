using UnityEngine;
using MazeGame;
using System;

public class SpeedDebuffController : MonoBehaviour
{
    [SerializeField] private GameObject _debuffEffect; //ヒットエフェクト
    [SerializeField] private SoundData _debuffSound; //デバフ音
    [SerializeField] private float _debuffRate = 0.5f; //デバフ倍率
    [SerializeField] private float debuffTime = 20f;
    GameObject player;


    private void OnDestroy()
    {
        if(player!= null)
        {
            PlayerController playerController = player.GetComponentInParent<PlayerController>();
            playerController.CompleateSpeedData(GetInstanceID().ToString());
        }
    }

    private void Update()
    {
        if (player == null) return;

        _debuffEffect.transform.position = player.transform.position;
    }
    void Initialize(GameObject _player)
    {
        if (_debuffEffect == null)
        {
            throw new InvalidOperationException("プレハブが未設定");
        }
        if (_debuffSound == null)
        {
            //throw new InvalidOperationException("サウンドが未設定");
        }
        //起動時に必要な初期化を行う
        _debuffEffect.gameObject.SetActive(false);
        //_debuffSound.Stop();
        player = _player;
    }

    public void Debuff(GameObject _player )
    {
        if (_player == null) return;
        Initialize(_player);

        PlayerController playerController = _player.GetComponentInParent<PlayerController>();
        playerController.DownSpeed(GetInstanceID().ToString(),_debuffRate);
        Destroy(gameObject,debuffTime);
    }
}
