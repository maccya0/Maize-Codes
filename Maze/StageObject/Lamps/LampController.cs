using UnityEngine;

namespace MazeGame
{
    public class LampController : MonoBehaviour
    {
        float amplitude; // êUïù
        float T;    //é¸ä˙
        [SerializeField] Light Light;

        private void OnBecameInvisible()
        {
            Light.enabled = false;
        }

        private void OnBecameVisible()
        {
            Light.enabled = true;
        }
        void Start()
        {
            float randam = (float)Random.Range(-10, 10) / 100.0f;
            transform.position += new Vector3(0, randam, 0);
            amplitude = (float)Random.Range(1, 3) / 100.0f;
            T = (float)Random.Range(3, 10);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float F = 1.0f / T;

            // è„â∫Ç…êUìÆÇ≥ÇπÇÈ
            float posYSin = Mathf.Sin(2.0f * Mathf.PI * F * Time.time);
            iTween.MoveAdd(gameObject, new Vector3(0, amplitude * posYSin, 0), 0.0f);
        }
    }

}
