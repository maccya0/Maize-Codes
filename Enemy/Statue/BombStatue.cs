using MazeGame;
using UnityEngine;

public class BombStatue : Statue
{
    [SerializeField] private GameObject BombPrefab;   //ボムプレハブ

    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        GameObject runObject = Instantiate(BombPrefab);
        runObject.transform.position = gameObject.transform.position;
        runObject.GetComponent<ExplosionController>().Explosion();
    }
}
