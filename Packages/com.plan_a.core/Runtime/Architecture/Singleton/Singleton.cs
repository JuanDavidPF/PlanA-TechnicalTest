using UnityEngine;

namespace PlanA.Architecture
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static private T _instance;

        static readonly private object _lock = new();

        static private bool _isQuitting;
        [SerializeField] private bool isPersistent;

        public static T Instance
        {
            get
            {
                if (_isQuitting)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (_instance != null) return _instance;
                    _instance = FindFirstObjectByType<T>();
                    if (_instance != null) return _instance;

                    GameObject go = new(typeof(T).Name);
                    _instance = go.AddComponent<T>();

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (isPersistent)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Debug.LogError($"Instance of {GetType().Name} already exists.", _instance);
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}