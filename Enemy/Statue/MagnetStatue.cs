using MazeGame;
using UnityEngine;
using System.Collections;

public class MagnetStatue : Statue
{
    [SerializeField] private float duration = 5.0f;   // 持続時間
    [SerializeField] private float radius = 5.0f;   // 効力の半径
    [SerializeField] private float power = 100.0f;   // 磁力


    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        StartCoroutine(MagneticForce(gameObject));
    }

    private IEnumerator MagneticForce(GameObject player)
    {
        float elapsedTime = duration;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        do
        {
            elapsedTime -= Time.deltaTime;
            // プレイヤー　→　像へのベクトルを生成
            Vector3 direction = this.transform.position - player.transform.position;
            float dist = direction.magnitude;
            // Max 1.0に丸める
            dist = Mathf.Max(dist, 1.0f);
            if(dist < radius)
            {
                // 距離で減衰させた磁力を算出
                float magnetForce = power/(dist*dist);
                // 像に集まるよう力を加える
                rb.AddForce(direction.normalized * magnetForce, ForceMode.Force);

            }
            yield return new WaitForFixedUpdate();
        } while (elapsedTime >0);

    }
}
