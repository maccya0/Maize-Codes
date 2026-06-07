using UnityEngine;

namespace MazeGame
{
    public class IceEffect : MonoBehaviour
    {
        [SerializeField] private short Damage = 600;
        [SerializeField] private float explosionImpact = 100f;
        [SerializeField] private float explosionRadius = 5.0f;
        [SerializeField] private SoundData effectSE;
        private bool isHit;

        private void Awake()
        {
            isHit = false;
            SoundManager.Instance.RequestSe(effectSE, this.transform.position);
        }

        void OnParticleCollision(GameObject other)
        {
            Debug.Log("IceParticele is Hit");
            if (isHit) return; 
            isHit = true;
            Debug.Log("IceParticele is Hit To Player");


            // îöî≠Ç…çáÇÌÇπÇƒÉ_ÉÅÅ[ÉWÇ∆êÅÇ¡îÚÇ—Çó^Ç¶ÇÈ
            var rigidBody = other.GetComponent<Rigidbody>();
            rigidBody.AddExplosionForce(explosionImpact, this.transform.position, explosionRadius);
            other.gameObject.GetComponent<PlayerController>().AddDamage(Damage,false);
        }

    }
}
