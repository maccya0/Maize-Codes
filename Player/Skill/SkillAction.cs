using MazeGame;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillAction : MonoBehaviour
{
    protected PlayerController playerController;
    protected InputSystem_Actions actions;
    [SerializeField] protected SkillData skillData;
    protected byte actionNum = 0;
    protected float recastTime = 0;
    protected bool isReady;
    protected GameObject particleObject;
    private Coroutine runninngCoroutine;


    [Header("-------------- スキルイベント通知 --------------")]
    [Tooltip("リキャストの進捗率（0.0 〜 1.0）を通知")]
    // Genraterが違う場合に備えてUnityEventで実施する
    [SerializeField] private UnityEvent<float> onRecastProgress = new UnityEvent<float>();

    [Tooltip("残り使用回数を通知")]
    [SerializeField] private UnityEvent<int> onActionSkill = new UnityEvent<int>();

    public UnityEvent<float> OnRecastProgress => onRecastProgress;
    public UnityEvent<int> OnActionSkill => onActionSkill;
    public virtual void Init(PlayerController _playerController, InputSystem_Actions _actions)
    {
        if (skillData == null)
        {
            throw new InvalidOperationException("データが未設定");
        }

        if (skillData.particle == null)
        {
            throw new InvalidOperationException("パーティクルが未設定");
        }
        if (skillData.actionTime == 0 || skillData.actionLimit == 0)
        {
            throw new InvalidOperationException("1以上の値が未設定");
        }
        this.playerController = _playerController;
        this.actions = _actions;
    }

    public virtual void Begin()
    {
        actionNum = skillData.actionLimit;
        isReady = true;
        recastTime = 0;
        runninngCoroutine = null;
        OnActionSkill?.Invoke(actionNum);
    }

    public abstract void Cleanup();

    public void StartAction()
    {
        if (!isReady || actionNum == 0 || !CanExecuteCustom()) return;
        if(runninngCoroutine != null)
        {
            StopCoroutine(runninngCoroutine);
        }
        runninngCoroutine = StartCoroutine(ActionFlow());
    }

    // 個別スキルごとの追加条件（スタミナ等）があればオーバーライド
    protected virtual bool CanExecuteCustom() => true;
    protected abstract IEnumerator ExecuteRoutine();

    protected void SkillExecute()
    {
        // スキル回数を減算
        actionNum--;
        OnActionSkill?.Invoke(actionNum);
        OnRecastProgress?.Invoke(0);
    }

    private IEnumerator ActionFlow()
    {
        isReady = false;
        yield return ExecuteRoutine();


        // 後に何回か使うのでキャッシュ
        float maxTime = skillData.recastTime;
        recastTime = maxTime;
        while (recastTime > 0)
        {
            recastTime -= Time.deltaTime;
            float rate = (maxTime - recastTime) / maxTime ;
            OnRecastProgress?.Invoke(rate);
            yield return null;
        }

        // 完了したのを通知
        OnRecastProgress?.Invoke(1);
        isReady = true;
    }

    public void StopAction()
    {
        if(runninngCoroutine != null && recastTime ==0)
        {
            StopCoroutine (runninngCoroutine);
            runninngCoroutine = null;
            if (particleObject != null)
            {
                Destroy(particleObject);
                particleObject = null;
            }
            isReady = true;
            OnActionCanceled();
        }
    }
    protected virtual void OnActionCanceled() { }

    protected void StartSe(SoundData sound,Vector3 position )
    {
        SoundManager soundManager = SoundManager.Instance;
        if(soundManager != null)
        {
            soundManager.RequestSe(sound, position);
        }
    }

    protected GameObject InstantiateAndDestroy(GameObject prefab, Vector3 position, Quaternion rotation, SoundData soundData)
    {
        GameObject go = Instantiate(prefab, position, rotation);

        StartSe(soundData, position);

        // ParticleSystemコンポーネントを取得
        var ps = go.GetComponent<ParticleSystem>();
        go.SetActive(true);
        if (ps != null)
        {
            // パーティクルのメインモジュールから「持続時間 + 寿命」を取得して、その後にDestroy
            float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
            Destroy(go, totalDuration);
        }
        else
        {
            // パーティクルでなければデフォルトの3秒とかで消す
            Destroy(go, 3.0f);
        }
        return go;
    }

    public void UseCharge(int addNum)
    {
        actionNum += (byte)addNum;
        if ( actionNum > skillData.actionLimit)
        {
            actionNum = skillData.actionLimit;
        }
        OnActionSkill?.Invoke(actionNum);
    }


}
