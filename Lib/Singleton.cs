using System;

//ジェネリックで実装するSingletonのスーパークラス
public abstract class Singleton<T> : IDisposable where T : class, new()
{
    //シングルトンパターンなのでprivate
    private static T instance = null;



    //Instance関数の糖衣構文
    //ラムダ式でコンストラクタの宣言を戻り値T 変数I 実行Instance に省略
    public static T I => Instance;
    //ラムダ式で実行される関数
    public static T Instance
    {
        get
        {
            CreateInstance();
            return instance;
        }
    }

    
    //instance生成
    public static void CreateInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
    }

    //instanceが生成されているかどうか
    public static bool isExsists()
    {
        return instance != null;
    }

    //インスタンス破棄
    public virtual void Dispose()
    {
        instance = null;
    }

}
