using System;
using System.Collections.Generic;
using PlanA.Architecture.Services;
using UnityEngine;

namespace PlanA.Architecture.EventBus
{
    public sealed class EventBusService : IGameService
    {
        readonly private Dictionary<Type, Delegate> _eventTable = new();

        public void Initialize()
        {
        }

        public void DeInitialize()
        {
            _eventTable.Clear();
        }

        public void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent
        {
            Type eventType = typeof(TEvent);

            if (_eventTable.TryGetValue(eventType, out Delegate existingDelegate))
            {
                _eventTable[eventType] = Delegate.Combine(existingDelegate, listener);
            }
            else
            {
                _eventTable.Add(eventType, listener);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent
        {
            Type eventType = typeof(TEvent);

            if (!_eventTable.TryGetValue(eventType, out Delegate existingDelegate))
                return;

            Delegate newDelegate = Delegate.Remove(existingDelegate, listener);

            if (newDelegate == null)
            {
                // No listeners left → remove event entirely
                _eventTable.Remove(eventType);
            }
            else
            {
                _eventTable[eventType] = newDelegate;
            }
        }

        public void Raise<TEvent>(TEvent gameEvent) where TEvent : IGameEvent
        {
            Debug.Log($"EventBusService::Raise: {gameEvent}");
            Type eventType = typeof(TEvent);

            if (!_eventTable.TryGetValue(eventType, out Delegate existingDelegate)) return;

            if (existingDelegate is Action<TEvent> callback)
            {
                callback.Invoke(gameEvent);
            }
        }
    }
}