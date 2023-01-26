using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public class HUDButtonsPresenter : HUDElementPresenter<HUDButtonsView>
    {
        [SerializeField]
        private HUDChannel m_HUDChannel;

        protected override void OnInit()
        {
            view.onRequestOpenCraft += m_HUDChannel.RaiseRequestCraftingWindow;
            view.onRequestOpenJournal += m_HUDChannel.RaiseRequestJournalWindow;
        }

        protected override void OnShutdown()
        {
            view.onRequestOpenCraft -= m_HUDChannel.RaiseRequestCraftingWindow;
            view.onRequestOpenJournal -= m_HUDChannel.RaiseRequestJournalWindow;
        }
    }
}
