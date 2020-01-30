using UnityEngine;

/// <summary>
/// Helper class for creating singletons scriptable objects in Unity.
/// Based off on this thread: <seealso cref="https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity#t=201705290349195977014"/>
/// </summary>
/// <typeparam name="T">The declaring singleton type</typeparam>
[CreateAssetMenu]
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{

    private static T m_instance = null;

    /// <summary>
    /// If set this singleton instance will be asigned only when needed.
    /// Default is true.
    /// </summary>
    public static bool Lazy { get; set; }

    /// <summary>
    /// Make this singleton instance persist through scenes.
    /// Default is false.
    /// </summary>
    public static bool Persist { get; set; }

    /// <summary>
    /// Get this singleton instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                var instances = Resources.FindObjectsOfTypeAll<T>();

                if (instances == null || instances.Length < 1)
                {
                    Instance = CreateInstance<T>();
                    m_instance.name = string.Format("{0} (Singleton)", typeof(T).Name);
                    Debug.LogFormat(Instance, "Created singleton instance of type '{0}' {1}", typeof(T).Name, Persist ? " with DontDestoryOnLoad" : string.Empty);
                }
                else if (instances.Length >= 1)
                {
                    Instance = instances[0];

                    if (instances.Length > 1)
                        Debug.LogWarningFormat("{0} instances of singleton object '{1}'!", instances.Length, typeof(T).Name);
                }
            }
            return m_instance;
        }
        private set
        {
            m_instance = value;
            m_instance.OnEnableSingleton();

            if (Persist)
                DontDestroyOnLoad(m_instance);
        }
    }

    static SingletonScriptableObject()
    {
        Lazy = true;
        Persist = false;
    }

    /// <summary>
    /// Remember to call base.Awake() if you override this implementation.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (Lazy)
            return;
    }

    /// <summary>
    /// This is called when one singleton instance is assigned.
    /// </summary>
    protected virtual void OnEnableSingleton() { }

}