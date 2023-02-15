using System;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class HUDButtonsView : HUDElementView
    {
        public event Action onRequestOpenCraft;
        public event Action onRequestOpenJournal;
        public event Action onRequestOpenProfile;

        private VisualElement m_CraftButton;
        private VisualElement m_JournalButton;
        private VisualElement m_ProfileButton;

        public override void Init()
        {
            m_CraftButton = root.Q<VisualElement>("btn_Craft");
            m_JournalButton = root.Q<VisualElement>("btn_Journal");
            m_ProfileButton = root.Q<VisualElement>("btn_Profile");

            m_CraftButton.RegisterCallback<MouseDownEvent>(OnCraftClick);
            m_JournalButton.RegisterCallback<MouseDownEvent>(OnJournalClick);
            m_ProfileButton.RegisterCallback<MouseDownEvent>(OnProfileClick);
        }

        public override void Shutdown()
        {
            m_CraftButton.UnregisterCallback<MouseDownEvent>(OnCraftClick);
            m_JournalButton.UnregisterCallback<MouseDownEvent>(OnJournalClick);
            m_ProfileButton.UnregisterCallback<MouseDownEvent>(OnProfileClick);

            m_CraftButton = null;
            m_JournalButton = null;
            m_ProfileButton = null;
        }

        private void OnCraftClick(MouseDownEvent mouseDownEvent)
        {
            onRequestOpenCraft?.Invoke();
        }

        private void OnJournalClick(MouseDownEvent mouseDownEvent)
        {
            onRequestOpenJournal?.Invoke();
        }

        private void OnProfileClick(MouseDownEvent mouseDownEvent)
        {
            onRequestOpenProfile?.Invoke();
        }
    }
}
