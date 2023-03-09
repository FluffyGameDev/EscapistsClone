using FluffyGameDev.Escapists.Crafting;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class JournalQuestTabView
    {
        private const int k_QuestPerPage = 3;

        public List<Quest.Quest> ActiveQuest { get; set; }

        private int m_CurrentPage;
        private int m_MaxPage;
        private List<Quest.Quest> m_DisplayedQuests = new(k_QuestPerPage);

        public VisualTreeAsset QuestVisualAsset { get; set; }

        private VisualElement m_Root;
        private ListView m_QuestListView;
        private Label m_PageNumberLabel;
        private Button m_PrevPageButton;
        private Button m_NextPageButton;

        public JournalQuestTabView(VisualElement root)
        {
            m_Root = root;
        }

        public void Init()
        {
            m_QuestListView = m_Root.Q<ListView>("lst_Quests");
            m_PageNumberLabel = m_Root.Q<Label>("lbl_PageNumber");
            m_PrevPageButton = m_Root.Q<Button>("btn_PageLeft");
            m_NextPageButton = m_Root.Q<Button>("btn_PageRight");

            m_QuestListView.makeItem = MakeQuestElement;
            m_QuestListView.bindItem = BindQuestElement;
            m_PrevPageButton.clicked += OnPagePrev;
            m_NextPageButton.clicked += OnPageNext;
        }

        public void Shutdown()
        {
            m_PrevPageButton.clicked -= OnPagePrev;
            m_NextPageButton.clicked -= OnPageNext;
        }

        public void Display()
        {
            m_Root.visible = true;
            m_Root.style.display = DisplayStyle.Flex;

            m_CurrentPage = 0;
            m_MaxPage = Math.Max((ActiveQuest.Count - 1) / k_QuestPerPage, 0);

            m_QuestListView.itemsSource = m_DisplayedQuests;
            RefreshQuestList();
            RefreshPageNumber();
        }

        public void Hide()
        {
            if (ActiveQuest != null)
            {
                m_CurrentPage = 0;
                m_MaxPage = Math.Max((ActiveQuest.Count - 1) / k_QuestPerPage, 0);

                m_QuestListView.itemsSource = m_DisplayedQuests;
                RefreshQuestList();
                RefreshPageNumber();
            }

            m_Root.visible = false;
            m_Root.style.display = DisplayStyle.None;
        }

        private void RefreshQuestList()
        {
            m_DisplayedQuests.Clear();

            int startIndex = m_CurrentPage * k_QuestPerPage;
            int maxIndex = Math.Min(startIndex + k_QuestPerPage, ActiveQuest.Count);
            for (int i = startIndex; i < maxIndex; ++i)
            {
                m_DisplayedQuests.Add(ActiveQuest[i]);
            }

            m_QuestListView.RefreshItems();
        }

        private void RefreshPageNumber()
        {
            m_PageNumberLabel.text = $"{m_CurrentPage + 1}/{m_MaxPage + 1}";
        }

        private void OnPagePrev()
        {
            if (m_CurrentPage > 0)
            {
                --m_CurrentPage;
            }

            RefreshPageNumber();
            RefreshQuestList();
        }

        private void OnPageNext()
        {
            if (m_CurrentPage < m_MaxPage)
            {
                ++m_CurrentPage;
            }

            RefreshPageNumber();
            RefreshQuestList();
        }

        private VisualElement MakeQuestElement()
        {
            return QuestVisualAsset.CloneTree();
        }

        private void BindQuestElement(VisualElement element, int index)
        {
            Quest.Quest quest = m_DisplayedQuests[index];
            Label questName = element.Q<Label>("lbl_QuestName");
            Label questObjective = element.Q<Label>("lbl_QuestObjective");

            questName.text = quest.QuestName;
            questObjective.text = "..."; //TODO: quest objective UI
        }
    }
}
