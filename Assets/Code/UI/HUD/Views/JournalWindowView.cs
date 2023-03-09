using FluffyGameDev.Escapists.Crafting;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class JournalWindowView : HUDElementView
    {
        private JournalCraftingTabView m_CraftingView;
        private JournalQuestTabView m_QuestView;


        public VisualTreeAsset RecipeVisualAsset
        {
            get => m_CraftingView.RecipeVisualAsset;
            set => m_CraftingView.RecipeVisualAsset = value;
        }

        public List<CraftingRecipeData> Recipes
        {
            get => m_CraftingView.Recipes;
            set => m_CraftingView.Recipes = value;
        }

        public VisualTreeAsset QuestVisualAsset
        {
            get => m_QuestView.QuestVisualAsset;
            set => m_QuestView.QuestVisualAsset = value;
        }

        public List<Quest.Quest> ActiveQuest
        {
            get => m_QuestView.ActiveQuest;
            set => m_QuestView.ActiveQuest = value;
        }

        public override void Init()
        {
            m_CraftingView = new JournalCraftingTabView(root.Q("ctr_CraftingNotes"));
            m_CraftingView.Init();

            m_QuestView = new JournalQuestTabView(root.Q("ctr_QuestLog"));
            m_QuestView.Init();

            root.Q("bt_CraftingTab").RegisterCallback<MouseDownEvent>(OnCraftingTabMouseDown);
            root.Q("bt_QuestTab").RegisterCallback<MouseDownEvent>(OnQuestTabMouseDown);
        }

        public override void Shutdown()
        {
            m_QuestView.Shutdown();
            m_CraftingView.Shutdown();
        }

        protected override void OnDisplay()
        {
            m_CraftingView.Display();
            m_QuestView.Hide();
        }

        protected override void OnHide()
        {
            m_CraftingView.Hide();
            m_QuestView.Hide();
        }

        private void OnCraftingTabMouseDown(MouseDownEvent e)
        {
            m_CraftingView.Display();
            m_QuestView.Hide();
        }

        private void OnQuestTabMouseDown(MouseDownEvent e)
        {
            m_QuestView.Display();
            m_CraftingView.Hide();
        }
    }
}
