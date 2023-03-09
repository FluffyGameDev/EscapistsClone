using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Crafting;
using FluffyGameDev.Escapists.Quest;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class JournalWindowPresenter : HUDElementPresenter<JournalWindowView>
    {
        [SerializeField]
        private HUDChannel m_HUDChannel;
        [SerializeField]
        private VisualTreeAsset m_RecipeVisualAsset;
        [SerializeField]
        private VisualTreeAsset m_QuestVisualAsset;

        private bool m_IsOpen;

        protected override void OnInit()
        {
            view.RecipeVisualAsset = m_RecipeVisualAsset;
            view.QuestVisualAsset = m_QuestVisualAsset;
            m_HUDChannel.onRequestJournalWindow += OnToggleView;
            m_HUDChannel.onRequestCraftingWindow += Close;
            m_HUDChannel.onRequestProfileWindow += Close;
        }

        protected override void OnShutdown()
        {
            m_HUDChannel.onRequestJournalWindow -= OnToggleView;
            m_HUDChannel.onRequestCraftingWindow -= Close;
            m_HUDChannel.onRequestProfileWindow -= Close;
        }

        private void OnToggleView()
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                view.Recipes = ServiceLocator.LocateService<ICraftingService>().knownRecipes;
                view.ActiveQuest = ServiceLocator.LocateService<IQuestService>().ActiveQuests;
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
    }
}
