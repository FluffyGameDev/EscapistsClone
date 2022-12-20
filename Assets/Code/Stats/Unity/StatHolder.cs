using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{
    public class StatHolder : MonoBehaviour
    {
        [SerializeField]
        private Stats.StatsContainerDescriptor m_StatsDescriptors;

        private Stats.StatsContainer m_Stats;
        public Stats.StatsContainer Stats => m_Stats;

        private void Awake()
        {
            m_Stats = m_StatsDescriptors.CreateStatsContainer();
        }
    }
}
