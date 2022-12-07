using UnityEngine;
using System;

namespace FluffyGameDev.Escapists.World
{
    [Serializable]
    public class Activity
    {
        [SerializeField]
        private int m_StartTime;
        [SerializeField]
        private string m_ActivityName;

        public int StartTime => m_StartTime;
        public string ActivityName => m_ActivityName;
    }
}
