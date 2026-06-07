using MazeGame;
using UnityEngine;

public class InfomationStatue : Statue
{
    private MessageScrollManager messageScrollManager;
    [SerializeField] float InfoRange = 50.0f;
    private void Start()
    {
        messageScrollManager = GameObject.FindFirstObjectByType<MessageScrollManager>();

    }

    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        EnemyManager.Instance.InformPlayerWithPos(this.gameObject.transform.position, InfoRange);
        if (messageScrollManager != null)
        {
            // 取得成功時の処理
            messageScrollManager.EnqueueMessage("プレイヤーの位置が周囲の敵に知らされた");
        }
    }
}
