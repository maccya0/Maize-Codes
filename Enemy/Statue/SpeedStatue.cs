using MazeGame;
using UnityEngine;

public class SpeedStatue : Statue
{
    [SerializeField] private GameObject SpeedDebuffPrefab;   //スピードデバフプレハブ

    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        GameObject runObject = Instantiate(SpeedDebuffPrefab);
        //y=0に調整
        Vector3 pos = gameObject.transform.position;
        runObject.transform.position = pos;
        runObject.GetComponent<DebuffCircleController>().Debuff(); 
    }

}
