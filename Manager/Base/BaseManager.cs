using UnityEngine;

public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
            }
            return instance;
        }
    }

    [Tooltip("チェックを入れると、シーン遷移してもこのマネージャーは破棄されなくなる")]
    [SerializeField] protected bool IsAllSecene = false;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if(IsAllSecene)
            {
                DontDestroyOnLoad(gameObject); 
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public virtual void ManagerInit()
    {

    }
    public virtual void ManagerStart()
    {

    }

    public virtual void ManagerDestroy()
    {
    }

}