using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{

    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Stats/Stats Container Descriptor")]
    public class StatsContainerDescriptor : ScriptableObject, StatsContainerData
    {
        [SerializeField]
        private List<StatDescriptor> m_Stats;

        public StatsContainer CreateStatsContainer()
        {
            StatsContainer statsContainer = new();
            foreach (StatData statData in m_Stats)
            {
                statsContainer.RegisterStat(statData);
            }
            return statsContainer;
        }
    }
}
