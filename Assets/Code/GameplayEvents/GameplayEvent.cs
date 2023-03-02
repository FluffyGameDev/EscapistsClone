using System;
using UnityEngine;

namespace FluffyGameDev.Escapists
{
    public interface IGameplayEvent
    {
    }

    public abstract class GameplayEvent : ScriptableObject, IGameplayEvent
    {
        public event Action OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }

    public abstract class GameplayEvent<T1> : ScriptableObject, IGameplayEvent
    {
        public event Action<T1> OnEventRaised;

        public void RaiseEvent(T1 argument1)
        {
            OnEventRaised?.Invoke(argument1);
        }
    }

    public abstract class GameplayEvent<T1, T2> : ScriptableObject, IGameplayEvent
    {
        public event Action<T1, T2> OnEventRaised;

        public void RaiseEvent(T1 argument1, T2 argument2)
        {
            OnEventRaised?.Invoke(argument1, argument2);
        }
    }
}
