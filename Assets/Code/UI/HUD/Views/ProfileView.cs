using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class ProfileView : HUDElementView
    {
        private ProgressBar m_IntelligenceProgressBar;
        private ProgressBar m_SpeedProgressBar;
        private ProgressBar m_StrengthProgressBar;

        public override void Init()
        {
            m_IntelligenceProgressBar = root.Q("ctr_IntelligenceStat").Q<ProgressBar>();
            m_SpeedProgressBar = root.Q("ctr_SpeedStat").Q<ProgressBar>();
            m_StrengthProgressBar = root.Q("ctr_StrengthStat").Q<ProgressBar>();
        }

        public override void Shutdown()
        {
            m_IntelligenceProgressBar = null;
            m_SpeedProgressBar = null;
            m_StrengthProgressBar = null;
        }

        public void UpdateIntelligenceStat(int value)
        {
            m_IntelligenceProgressBar.value = value;
        }

        public void UpdateSpeedStat(int value)
        {
            m_SpeedProgressBar.value = value;
        }

        public void UpdateStrengthStat(int value)
        {
            m_StrengthProgressBar.value = value;
        }
    }
}
