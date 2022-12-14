using FluffyGameDev.Escapists.Core;
using UnityEngine;
using System;

namespace FluffyGameDev.Escapists.World
{
    public struct DateTime
    {
        public int Day;
        public int Hour;
        public int Minute;
    }

    public interface ITimeService : IService
    {
        DateTime CurrentTime { get; }
        event Action<DateTime> OnTimeChanges;
    }

    public class TimeService : MonoBehaviour, ITimeService
    {
        [SerializeField]
        [Range(0.001f, 1.0f)]
        private float m_TimeRatio = 1.0f;

        private DateTime m_CurrentTime;
        public DateTime CurrentTime => m_CurrentTime;

        public event Action<DateTime> OnTimeChanges;

        private float m_LastTimeUpdate = 0;
        private int m_RawTime = 0;

        private void Awake()
        {
            ServiceLocator.RegisterService<ITimeService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<ITimeService>(this);
        }

        private void Update()
        {
            float elapsedTime = (Time.time - m_LastTimeUpdate);
            if (elapsedTime >= m_TimeRatio)
            {
                m_LastTimeUpdate += m_TimeRatio;
                ++m_RawTime;

                m_CurrentTime = new()
                {
                    Minute = m_RawTime % 60,
                    Hour = (m_RawTime / 60) % 24,
                    Day = m_RawTime / 60 / 24 + 1
                };
                OnTimeChanges?.Invoke(m_CurrentTime);
            }
        }

        public void Init()
        {
            m_RawTime = 6 * 60; //TODO: remove Magic number
        }

        public void Shutdown()
        {
        }
    }
}
