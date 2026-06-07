using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;
using System.Collections;
using System;
using System.Linq;

namespace MazeGame
{
    public class WarpPlane : PlaneTrap
    {
        [SerializeField] GameObject warparticle;
        [SerializeField] int removeMaterialIndex = 1;
        [SerializeField] SoundData warpSound;

        private void Awake()
        {
            if (warparticle == null)
            {
                throw new InvalidOperationException("ワーププレハブが未設定");
            }
        }

        private IEnumerator StartBeforeEffect(GameObject instance,GameObject player, Vector3 warpPos)
        {
            float particleTime = instance.GetComponent<ParticleSystem>().main.duration;
            yield return new WaitForSeconds(particleTime/2);            
            player.transform.position = warpPos;
            Destroy(instance);
        }
        private IEnumerator StartAfterEffect(GameObject instance)
        {
            float particleTime = instance.GetComponent<ParticleSystem>().main.duration;
            yield return new WaitForSeconds(particleTime);
            Destroy(instance);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (endFlg) return;
            if (collision.gameObject.layer == LayerMask.NameToLayer(PlayerConstants.Layer))
            {
                endFlg = true;
                Maze mazeObj = Maze.Instance;
                int stageSize = mazeObj.GetStageSize();
                MazeObjKinds[,] maze = mazeObj.GetMazeData();
                int posX;
                int posY;
                do
                {
                    posX = UnityEngine.Random.Range(0, stageSize - 1);
                    posY = UnityEngine.Random.Range(0, stageSize - 1);
                } while (maze[posX, posY] != MazeObjKinds.EPath);

                Vector3 WarpPos = Maze.Instance.GetObjectPos(posX, posY);
                // 少し高い場所に移動する事ですり抜けを防止する
                WarpPos += new Vector3(0, 0.1f, 0);
                SoundManager soundManager = SoundManager.Instance;
                if(soundManager != null)
                {
                    soundManager.RequestSe(warpSound, this.transform.position);
                }
                GameObject beforeEffect = Instantiate(warparticle, collision.gameObject.transform.position, Quaternion.identity);
                StartCoroutine(StartBeforeEffect(beforeEffect, collision.gameObject, WarpPos));
                GameObject afterEffect = Instantiate(warparticle, WarpPos, Quaternion.identity);
                StartCoroutine(StartAfterEffect(afterEffect));
                RemoveMaterialAtIndex(removeMaterialIndex);
            }
        }

        private void RemoveMaterialAtIndex(int index)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer == null) return;

            var materialList = renderer.materials.ToList();

            if (index >= 0 && index < materialList.Count)
            {
                // 指定した番号の要素を消す
                materialList.RemoveAt(index);
                renderer.materials = materialList.ToArray();
            }
        }


    }

}
