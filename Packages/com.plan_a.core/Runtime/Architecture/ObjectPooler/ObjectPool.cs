using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA.Architecture.Architecture.ObjectPool
{
    [Serializable]
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public Transform EnqueuedContainer { get; private set; }

        [field: SerializeField] public T Prefab { get; private set; }

        [field: SerializeField] public int PoolSize { get; private set; }

        readonly private Queue<T> _pool = new();
        private bool _initialized;

        public void Initialize()
        {
            if (_initialized) return;

            if (Prefab == null)
            {
                Debug.LogError($"[ObjectPool<{typeof(T).Name}>] Prefab is null.");
                return;
            }

            if (EnqueuedContainer == null)
            {
                EnqueuedContainer = new GameObject($"{typeof(T).Name}_Pool").transform;
            }

            for (int i = 0; i < PoolSize; i++)
            {
                T instance = CreateInstance();
                Enqueue(instance);
            }

            _initialized = true;
        }

        private T CreateInstance()
        {
            T instance = UnityEngine.Object.Instantiate(Prefab, EnqueuedContainer);
            instance.gameObject.SetActive(false);
            return instance;
        }

        public T Dequeue(Transform parent = null)
        {
            if (!_initialized) Initialize();

            T instance = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();

            instance.transform.SetParent(parent, false);
            instance.gameObject.SetActive(true);
            instance.OnDequeue();

            return instance;
        }

        public void Enqueue(T instance)
        {
            instance.OnEnqueue();
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(EnqueuedContainer, false);

            _pool.Enqueue(instance);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                T instance = _pool.Dequeue();

                if (instance != null)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
            }

            _pool.Clear();
            _initialized = false;
        }
    }
}