using MazeGame;
using UnityEngine;
using static MazeGame.MazeGameConstants;

public class HitParticle : MonoBehaviour
{
    [SerializeField] private short hitDamege = 200;
    [SerializeField] private int hitImpact = 500;
    bool ishit = false;

    void OnParticleCollision(GameObject other)
    {
        if (ishit) return;
        if (other.tag == PlayerConstants.Tag)
        {
            ishit = true;
            //ダメージを加える
            other.GetComponentInParent<PlayerController>().AddDamage(hitDamege);
            //レーザーの方向に吹き飛ばす
            other.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * hitImpact);
        }
    }
}
