

using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public class StatsPresenter : HUDElementPresenter<StatsView>
    {
        [SerializeField]
        private Stats.StatHolder m_StatHolder;
        [SerializeField]
        private Stats.StatDescriptor m_HealthStat;
        [SerializeField]
        private Stats.StatDescriptor m_StaminaStat;
        [SerializeField]
        private Stats.StatDescriptor m_HeatStat;

        protected override void OnInit()
        {
            view.Setup(m_StatHolder.Stats, m_HealthStat, m_StaminaStat, m_HeatStat);
        }
    }
}
