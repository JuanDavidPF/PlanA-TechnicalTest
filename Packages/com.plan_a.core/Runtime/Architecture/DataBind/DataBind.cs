using System;
using UnityEngine;

namespace PlanA.Architecture.DataBinding
{
    [Serializable]
    public class DataBind<TDataBind, TPayload> where TDataBind : DataBind<TDataBind, TPayload>
    {
        [field: SerializeField] public TPayload Value { private set; get; }

        public DataBind(TPayload value)
        {
            Value = value;
        }

        public TDataBind SetValue(TPayload value, bool notify = true)
        {
            Value = value;

            if (notify)
            {
                OnChanged?.Invoke((TDataBind)this);
            }

            return (TDataBind)this;
        }

        private event Action<TDataBind> OnChanged;

        public TDataBind Bind(Action<TDataBind> callback, bool raiseCallbackOnBind = false)
        {
            OnChanged -= callback;
            OnChanged += callback;

            if (raiseCallbackOnBind)
            {
                callback?.Invoke((TDataBind)this);
            }

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