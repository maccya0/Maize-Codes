using System;
using Unity.AI.Navigation;
using UnityEngine;

public abstract class BaseGenerator : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void Generated() { }

    public virtual void Tick() { }
    public virtual void Destroy() { }

    public virtual void EndGame() { }

}

