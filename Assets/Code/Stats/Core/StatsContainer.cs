using System.Collections.Generic;

namespace FluffyGameDev.Escapists.Stats
{
    public class StatsContainer
    {
        private Dictionary<StatData, Stat> m_Stats = new();

        public void RegisterStat(StatData data)
        {
            m_Stats.Add(data, data.CreateStat());
        }

        public Stat GetStat(StatData data)
        {
            m_Stats.TryGetValue(data, out Stat foundStat);
            if (foundStat == null) //TODO: ifdef debug
            {
                //TODO: log error
            }
            return foundStat;
        }
    }
}
