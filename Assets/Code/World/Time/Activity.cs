using UnityEngine;
using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.World
{
    [Serializable]
    public class Activity
    {
        [SerializeField]
        private string m_ActivityName;
        [SerializeField]
        private List<ActivityIdentifier> m_ActivityIdentifiers;
        [SerializeField]
        private int m_StartTime;

        public string ActivityName => m_ActivityName;
        public List<ActivityIdentifier> ActivityIdentifiers => m_ActivityIdentifiers;
        public int StartTime => m_StartTime;
    }
}
