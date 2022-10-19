using System;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Core
{
    public class ServiceLocator : MonoBehaviour
    {
        public static T LocateService<T>()
        {
            return (T)ms_Instance.m_Services[typeof(T)];
        }

        public static void RegisterService<T>(T service) where T : IService
        {
            if (ms_Instance.m_Services.TryAdd(typeof(T), service))
            {
                service.Init();
            }
            else
            {
                Debug.LogError($"Tried to register more than one service of type '{service.GetType().FullName}'.");
            }
        }

        public static void UnregisterService<T>(T service) where T : IService
        {
            if (ms_Instance.m_Services.Remove(typeof(T), out IService removedService))
            {
                removedService.Shutdown();
            }
            else
            {
                Debug.LogError($"Tried to unregister a service of type '{service.GetType().FullName}' that was not registered.");
            }
        }



        private static ServiceLocator ms_Instance;

        private Dictionary<Type, IService> m_Services;

        private void Awake()
        {
            if (ms_Instance == null)
            {
                ms_Instance = this;
            }
            else
            {
                Debug.LogError("Tried to register more than one service locator.");
            }
        }

        private void OnDestroy()
        {
            if (ms_Instance == this)
            {
                ms_Instance = null;
            }
        }
    }
}
