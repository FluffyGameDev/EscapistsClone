using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Crafting;
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

        private bool m_IsOpen;

        protected override void OnInit()
        {
            view.RecipeVisualAsset = m_RecipeVisualAsset;
            m_HUDChannel.onRequestJournalWindow += OnToggleView;
        }

        protected override void OnShutdown()
        {
            m_HUDChannel.onRequestJournalWindow -= OnToggleView;
        }

        private void OnToggleView()
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                view.Recipes = ServiceLocator.LocateService<ICraftingService>().knownRecipes;
                view.Display();
            }
            else
            {
                view.Hide();
            }
        }
    }
}
