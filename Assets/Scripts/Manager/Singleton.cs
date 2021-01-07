/// <summary>
/// 必须显式销毁单例，会根据生命周期类型判断哪些单例没有处理生命周期问题
/// </summary>
/// <typeparam name = "T"></typeparam>
public interface ISingleton
{
    string GetSingletonName();
    void DestroyInstance();
    void Reset();
    void Awake();
    void Update();
    void LateUpdate();
}

public abstract class Singleton<T> :ISingleton where T : Singleton<T>, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                CreateInstance();
            }

            return _instance;
        }
    }

    protected Singleton()
    {
    }

    static private void CreateInstance()
    {
        if (_instance != null)
        {
            Log.LogDebug( "Instance is already exist:" + typeof(T));
            return;
        }

        _instance = new T();
    }

    public virtual void Awake()
    {
    }

    public virtual void Reset()
    {
    }


    public virtual void Update()
    {
    }


    public virtual void LateUpdate()
    {
    }

    // 可显式调用释放接口，否则会在管理器中调用
    public void DestroyInstance()
    {
        if (_instance == null)
        {
            return;
        }

        if (_instance != this)
        {
            return;
        }

        _instance = default(T);
    }

    static public bool IsInstanceExist()
    {
        return _instance != null && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_instance, default(T));
    }


    public virtual string GetSingletonName()
    {
        return "";
    }
}