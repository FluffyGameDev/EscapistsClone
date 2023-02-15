using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Stats;
using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public class ProfilePresenter : HUDElementPresenter<ProfileView>
    {
        [SerializeField]
        private HUDChannel m_HUDChannel;
        [SerializeField]
        private StatDescriptor m_IntelligenceStat;
        [SerializeField]
        private StatDescriptor m_SpeedStat;
        [SerializeField]
        private StatDescriptor m_StrengthStat;

        private bool m_IsOpen;

        protected override void OnInit()
        {
            m_HUDChannel.onRequestProfileWindow += OnToggleView;
            m_HUDChannel.onRequestCraftingWindow += Close;
            m_HUDChannel.onRequestJournalWindow += Close;
        }

        protected override void OnShutdown()
        {
            m_HUDChannel.onRequestProfileWindow -= OnToggleView;
            m_HUDChannel.onRequestCraftingWindow -= Close;
            m_HUDChannel.onRequestJournalWindow -= Close;
        }

        private void OnToggleView()
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                UpdateStatValues();
                view.Display();
            }
            else
            {
                view.Hide();
            }
        }

        private void Close()
        {
            m_IsOpen = false;
            view.Hide();
        }

        private void UpdateStatValues()
        {
            StatsContainer stats = ServiceLocator.LocateService<IPlayerStatsService>().PlayerStatsContainer;
            view.UpdateIntelligenceStat(stats.GetStat(m_IntelligenceStat).GetValueInt());
            view.UpdateSpeedStat(stats.GetStat(m_SpeedStat).GetValueInt());
            view.UpdateStrengthStat(stats.GetStat(m_StrengthStat).GetValueInt());
        }
    }
}
