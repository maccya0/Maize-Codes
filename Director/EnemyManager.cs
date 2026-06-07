using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                // シーン上から自動検索（念のためのセーフティ）
                instance = FindFirstObjectByType<EnemyManager>();
            }
            return instance;
        }
    }
    [Header("--- References ---")]
    [SerializeField] private List<GameObject> Enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> Statues = new List<GameObject>();
    [SerializeField] private List<GameObject> PatrolPoints = new List<GameObject>();
    [SerializeField] private GameObject Player;

    private List<Vector3> SpownPoints;
    private List<GameObject> EnemyList;
    private List<GameObject> StatueList;

    private void Awake()
    {
        instance = this;
        EnemyList = new List<GameObject>();
        StatueList = new List<GameObject>();
        SpownPoints = new List<Vector3>();
    }

    public void RegisterSpownPoints(Vector3 pos)
    {
        SpownPoints.Add(pos);
    }

    public void DeleatePatrolPoint(GameObject point)
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null) continue;

            if (EnemyList[i].TryGetComponent<CollectMazeInfo>(out var info))
            {
                info.RemovePatrolPoint(point);
            }
        }
    }
    public void DeleatePatrolPoint(GameObject enemy , GameObject point)
    {
        if (enemy == null) return;
        if (point == null) return;
        if (!EnemyList.Contains(enemy)) return;

        if (enemy.TryGetComponent<CollectMazeInfo>(out var info))
        {
            info.RemovePatrolPoint(point);
        }
    }
    public void AddPatrolPoint(GameObject Target)
    {
        if (Target == null) return;
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null) continue;

            if (EnemyList[i].TryGetComponent<CollectMazeInfo>(out var info))
            {
                info.RegisterPatrolPoint(Target);
            }
        }
    }
    public void AddPatrolPoint(GameObject enemy, GameObject Target)
    {
        if (enemy == null) return;
        if (Target == null) return;
        if (!EnemyList.Contains(enemy)) return;
        if (enemy.TryGetComponent<CollectMazeInfo>(out var info))
        {
            info.RegisterPatrolPoint(Target);
        }
    }

    public GameObject GenerateEnemy()
    {
        // スポーン地点のランダムな場所からランダムな敵を生成する
        if (Enemies == null || Enemies.Count == 0) return null;
        if (SpownPoints == null || SpownPoints.Count == 0) return null;
        int indexEnemy = UnityEngine.Random.Range(0, Enemies.Count);
        int indexPos = UnityEngine.Random.Range(0, SpownPoints.Count);
        GameObject enemy = Instantiate(Enemies[indexEnemy], SpownPoints[indexPos], Quaternion.identity);
        EnemyList.Add(enemy);
        InitializeEnemyDataDelayed(enemy);
        return enemy;
    }

    public GameObject GenerateStatue()
    {
        if (Statues == null || Statues.Count == 0) return null;
        if (SpownPoints == null || SpownPoints.Count == 0) return null;
        int indexStatue = UnityEngine.Random.Range(0, Statues.Count);
        int indexPos = UnityEngine.Random.Range(0, SpownPoints.Count);
        GameObject statue = Instantiate(Statues[indexStatue], SpownPoints[indexPos], Quaternion.identity);
        StatueList.Add(statue);
        SpownPoints.RemoveAt(indexPos);
        return statue;
    }

    public GameObject GenerateEnemy(GameObject enemyPrefab)
    {
        // プレハブがCollectMazeInfoを持っていないならEnemyではない
        if (enemyPrefab == null || enemyPrefab.GetComponent<CollectMazeInfo>() == null ) return null;
        // スポーン地点のランダムな場所からランダムな敵を生成する
        if (SpownPoints == null || SpownPoints.Count == 0) return null;
        int indexPos = UnityEngine.Random.Range(0, SpownPoints.Count);
        GameObject enemy = Instantiate(enemyPrefab, SpownPoints[indexPos], Quaternion.identity);
        EnemyList.Add(enemy);
        InitializeEnemyDataDelayed(enemy);
        return enemy;
    }

    private async void InitializeEnemyDataDelayed(GameObject enemy)
    {
        // 非同期処理でAgent生成まで待機させる
        CollectMazeInfo info = enemy.GetComponent<CollectMazeInfo>();
        do
        {
            // Agent生成まで待機
            await Task.Yield();

        } while (!info.IsGeneratedAgent());
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            info.RegisterPatrolPoint(PatrolPoints[i]);
        }
        info.RegisterPlayer(Player);
        info.ComplateInitialize();
    }

    // プレイヤーの位置を全エネミーに通知する
    public void InformPlayer()
    {
        if (Player == null) return;
        if (EnemyList == null) return;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null) continue;

            CollectMazeInfo info = EnemyList[i].GetComponent<CollectMazeInfo>();
            if (info != null)
            {
                info.InformPlayerPos(Player.gameObject.transform.position);
            }
        }
    }

    // プレイヤーの位置を一定範囲内のエネミーに通知する
    public void InformPlayerWithPos(Vector3 infoPos, float range = 20.0f)
    {
        if (Player == null) return;
        if (EnemyList == null) return;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null) continue;
            if(Vector3.Distance(EnemyList[i].transform.position,infoPos) >= range) continue;
            CollectMazeInfo info = EnemyList[i].GetComponent<CollectMazeInfo>();
            if (info != null)
            {
                info.InformPlayerPos(Player.gameObject.transform.position);
            }
        }
    }

    public int GetSpownPointNum()
    {
        return SpownPoints.Count;
    }
}