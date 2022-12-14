using FluffyGameDev.Escapists.Core;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.World
{
    public interface IScheduleService : IService
    {
        Activity CurrentActivity { get; }
        event Action<Activity> OnActivityChange;
    }

    public class ScheduleService : MonoBehaviour, IScheduleService
    {
        [SerializeField]
        private List<Activity> m_Activities;

        private Activity m_CurrentActivity;
        public Activity CurrentActivity => m_CurrentActivity;

        public event Action<Activity> OnActivityChange;

        private void Awake()
        {
            ServiceLocator.PreRegisterDependency<IScheduleService, ITimeService>();
            ServiceLocator.RegisterService<IScheduleService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<IScheduleService>();
        }

        public void Init()
        {
            ITimeService timeService = ServiceLocator.LocateService<ITimeService>();
            timeService.OnTimeChanges += OnTimeChange;
            m_CurrentActivity = FindCurrentActivity(timeService.CurrentTime);
        }

        public void Shutdown()
        {
        }

        private void OnTimeChange(DateTime time)
        {
            Activity nextActivity = FindCurrentActivity(time);
            if (m_CurrentActivity != nextActivity)
            {
                m_CurrentActivity = nextActivity;
                OnActivityChange?.Invoke(m_CurrentActivity);
            }
        }

        private Activity FindCurrentActivity(DateTime time)
        {
            Activity currentActivity = null;
            int index = 0;
            while (index < m_Activities.Count && m_Activities[index].StartTime <= time.Hour)
            {
                currentActivity = m_Activities[index];
                ++index;
            }
            return currentActivity;
        }
    }
}
