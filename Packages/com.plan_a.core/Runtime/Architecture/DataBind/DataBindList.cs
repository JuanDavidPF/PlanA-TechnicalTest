using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA.Architecture.DataBinding
{
    [Serializable]
    public class DataBindList<TDataBind, TPayload> where TDataBind : DataBindList<TDataBind, TPayload>
    {
        [SerializeField]
        private List<TPayload> _items = new();

        public DataBindList(IEnumerable<TPayload> initialItems)
        {
            _items.AddRange(initialItems);
        }

        public IReadOnlyList<TPayload> Items => _items;

        public event Action<TDataBind> OnChanged;

        public TDataBind Add(TPayload item, bool notify = true)
        {
            _items.Add(item);

            if (notify)
            {
                OnChanged?.Invoke((TDataBind)this);
            }

            return (TDataBind)this;
        }

        public TDataBind Remove(TPayload item, bool notify = true)
        {
            if (!_items.Remove(item))
                return (TDataBind)this;

            if (notify)
            {
                OnChanged?.Invoke((TDataBind)this);
            }

            return (TDataBind)this;
        }

        public TDataBind Clear(bool notify = true)
        {
            if (_items.Count == 0)
                return (TDataBind)this;

            _items.Clear();

            if (notify)
                OnChanged?.Invoke((TDataBind)this);

            return (TDataBind)this;
        }

        public TDataBind SetItems(IEnumerable<TPayload> items, bool notify = true)
        {
            _items.Clear();
            _items.AddRange(items);

            if (notify)
                OnChanged?.Invoke((TDataBind)this);

            return (TDataBind)this;
        }

        public TDataBind Bind(Action<TDataBind> callback, bool raiseCallbackOnBind)
        {
            OnChanged -= callback;
            OnChanged += callback;

            if (raiseCallbackOnBind)
                callback?.Invoke((TDataBind)this);

            return (TDataBind)this;
        }

        public TDataBind UnBind(Action<TDataBind> callback)
        {
            OnChanged -= callback;
            return (TDataBind)this;
        }

        public TDataBind ForceOnChangeNotification()
        {
            OnChanged?.Invoke((TDataBind)this);
            return (TDataBind)this;
        }
    }
}