using System;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Core
{
    public class ServiceLocator : MonoBehaviour
    {
        public static T LocateService<T>()
        {
            return (T)ms_Instance.m_ServicesData[typeof(T)].Service;
        }

        public static void RegisterService<T>(T service) where T : IService
        {

            if (!ms_Instance.m_ServicesData.TryGetValue(typeof(T), out ServiceData foundServiceData))
            {
                foundServiceData = new();
                ms_Instance.m_ServicesData[typeof(T)] = foundServiceData;
            }

            foundServiceData.Service = service;

            if (!foundServiceData.Ready)
            {
                ms_Instance.CheckCanInitService(typeof(T));
            }
            else
            {
                Debug.LogError($"Tried to register more than one service of type '{service.GetType().FullName}'.");
            }
        }

        public static void UnregisterService<T>() where T : IService
        {
            ms_Instance.TryShutdownService(typeof(T));
        }

        public static void PreRegisterDependency<WaitingService, ServiceDependency>()
            where WaitingService : IService
            where ServiceDependency : IService
        {
            if (!ms_Instance.m_ServicesData.TryGetValue(typeof(WaitingService), out ServiceData foundWaitingServiceData))
            {
                foundWaitingServiceData = new();
                ms_Instance.m_ServicesData[typeof(WaitingService)] = foundWaitingServiceData;
            }
            if (!ms_Instance.m_ServicesData.TryGetValue(typeof(ServiceDependency), out ServiceData foundServiceDependencyData))
            {
                foundWaitingServiceData = new();
                ms_Instance.m_ServicesData[typeof(ServiceDependency)] = foundServiceDependencyData;
            }

            foundWaitingServiceData.Dependencies.Add(typeof(ServiceDependency));
            foundServiceDependencyData.BlockedServices.Add(typeof(WaitingService));
        }

        public static void WaitUntilReady<T>(Action onServiceReady)
        {
            if (!ms_Instance.m_ServicesData.TryGetValue(typeof(T), out ServiceData foundServiceData))
            {
                foundServiceData = new();
                ms_Instance.m_ServicesData[typeof(T)] = foundServiceData;
            }

            if (foundServiceData.Ready)
            {
                onServiceReady?.Invoke();
            }

            foundServiceData.OnServiceReady += onServiceReady;
        }



        private static ServiceLocator ms_Instance;

        private class ServiceData
        {
            public List<Type> Dependencies = new ();
            public List<Type> BlockedServices = new();
            public Action OnServiceReady;
            public bool Ready = false;
            public IService Service = null;
        }

        private Dictionary<Type, ServiceData> m_ServicesData = new();

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

        private bool IsServiceReady(Type serviceType)
        {
            return ms_Instance.m_ServicesData.TryGetValue(serviceType, out ServiceData foundServiceData) && foundServiceData.Ready;
        }

        private bool AreAllDependenciesReady(List<Type> dependencies)
        {
            return dependencies.TrueForAll((serviceType) => IsServiceReady(serviceType));
        }

        private void CheckCanInitService(Type serviceType)
        {
            if (ms_Instance.m_ServicesData.TryGetValue(serviceType, out ServiceData foundServiceData)
                && foundServiceData.Service != null && !foundServiceData.Ready && AreAllDependenciesReady(foundServiceData.Dependencies))
            {
                foundServiceData.Service.Init();
                foundServiceData.Ready = true;

                foreach (Type blockedService in foundServiceData.BlockedServices)
                {
                    CheckCanInitService(blockedService);
                }

                foundServiceData.OnServiceReady?.Invoke();
            }
        }

        private void TryShutdownService(Type serviceType)
        {
            if (ms_Instance.m_ServicesData.Remove(serviceType, out ServiceData foundServiceData) && foundServiceData.Ready)
            {
                foreach (Type blockedService in foundServiceData.BlockedServices)
                {
                    TryShutdownService(blockedService);
                }
                foundServiceData.Service.Shutdown();
            }
        }
    }
}
