using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MazeGame;
using System;
using System.Threading.Tasks;

public class StartMenuDirector : BaseDirector<StartMenuDirector>
{
    private SystemDirector SystemDirector;

    public async void Init()
    {

        SystemDirector = UnityEngine.Object.FindAnyObjectByType<SystemDirector>();
        if (SystemDirector == null)
        {
            throw new Exception("Not Find A SystemDirector");
        }
        DirectorInit();
        await DirectorStart();

    }

    public void Tick()
    {
        DirectorRunTime();
    }

    public void Destroy()
    {
        DirectorDestroy();
    }

    protected override void DirectorInit()
    {
    }
    protected override Task DirectorStart()
    {
        return Task.CompletedTask;
    }

    protected override void DirectorRunTime()
    {
    }

    protected override Task DirectorDestroy()
    {
        return Task.CompletedTask;
    }

}

}
