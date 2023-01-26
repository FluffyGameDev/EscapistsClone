using System;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class HUDButtonsView : HUDElementView
    {
        public event Action onRequestOpenCraft;
        public event Action onRequestOpenJournal;

        private VisualElement m_CraftButton;
        private VisualElement m_JournalButton;

        public override void Init()
        {
            m_CraftButton = root.Q<VisualElement>("btn_Craft");
            m_JournalButton = root.Q<VisualElement>("btn_Journal");

            m_CraftButton.RegisterCallback<MouseDownEvent>(OnCraftClick);
            m_JournalButton.RegisterCallback<MouseDownEvent>(OnJournalClick);
        }

        public override void Shutdown()
        {
            m_CraftButton.UnregisterCallback<MouseDownEvent>(OnCraftClick);
            m_JournalButton.UnregisterCallback<MouseDownEvent>(OnJournalClick);

            m_CraftButton = null;
            m_JournalButton = null;
        }

        private void OnCraftClick(MouseDownEvent mouseDownEvent)
        {
            onRequestOpenCraft?.Invoke();
        }

        private void OnJournalClick(MouseDownEvent mouseDownEvent)
        {
            onRequestOpenJournal?.Invoke();
        }
    }
}
