using UnityEngine;

namespace MazeGame
{
    public class UICursorContoroller : MonoBehaviour
    {
        [SerializeField] GameObject cursor;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (cursor == null) return;
            cursor.SetActive(false);
        }

        public void OnSelect()
        {
            if (cursor == null) return;
            cursor.SetActive(true);
        }

        // Update is called once per frame
        public void OnDeSelect()
        {
            if (cursor == null) return;
            cursor.SetActive(false);

        }
    }

}
