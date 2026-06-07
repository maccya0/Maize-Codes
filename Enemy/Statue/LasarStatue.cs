using MazeGame;
using UnityEngine;
using static MazeGame.MazeGameConstants;

public class LasarStatue : Statue
{
    [SerializeField] private GameObject lasarPrefab;   //レーザープレハブ

    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        Vector3 pos = transform.position + transform.forward;
        pos.y = PlayerConstants.Height;
        GameObject runObject = Instantiate(lasarPrefab, pos, Quaternion.identity);
        runObject.transform.LookAt(gameObject.transform);
        LaserContoroller temp = runObject.GetComponent<LaserContoroller>();
        temp.Init();
        temp.Lasar();
    }
}
