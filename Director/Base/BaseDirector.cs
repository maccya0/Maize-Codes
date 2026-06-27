using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MazeGame.MazeGameConstants.MazeConstants;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;


namespace MazeGame
{
    public abstract class BaseDirector<T> : MonoBehaviour where T : class
    {

        protected abstract void DirectorInit();

        protected abstract Task DirectorDestroy();
        protected abstract Task DirectorStart();

        protected abstract void DirectorRunTime();
    }
}
