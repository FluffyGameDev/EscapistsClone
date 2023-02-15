using FluffyGameDev.Escapists.Crafting;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class JournalWindowView : HUDElementView
    {
        private const int k_RecipesPerPage = 2;

        public List<CraftingRecipeData> Recipes { get; set; }
        public VisualTreeAsset RecipeVisualAsset { get; set; }

        private int m_CurrentPage;
        private int m_MaxPage;
        private List<CraftingRecipeData> m_DisplayedRecipes = new(k_RecipesPerPage);

        private ListView m_RecipeListView;
        private Label m_PageNumberLabel;
        private Button m_PrevPageButton;
        private Button m_NextPageButton;

        public override void Init()
        {
            m_RecipeListView = root.Q<ListView>("lst_Recipes");
            m_PageNumberLabel = root.Q<Label>("lbl_PageNumber");
            m_PrevPageButton = root.Q<Button>("btn_PageLeft");
            m_NextPageButton = root.Q<Button>("btn_PageRight");

            m_RecipeListView.makeItem = MakeRecipeElement;
            m_RecipeListView.bindItem = BindRecipeElement;
            m_PrevPageButton.clicked += OnPagePrev;
            m_NextPageButton.clicked += OnPageNext;
        }

        public override void Shutdown()
        {
            m_PrevPageButton.clicked -= OnPagePrev;
            m_NextPageButton.clicked -= OnPageNext;
        }

        protected override void OnDisplay()
        {
            m_CurrentPage = 0;
            m_MaxPage = Math.Max((Recipes.Count - 1) / k_RecipesPerPage, 0);

            m_RecipeListView.itemsSource = m_DisplayedRecipes;
            RefreshRecipeList();
            RefreshPageNumber();
        }

        private void RefreshRecipeList()
        {
            m_DisplayedRecipes.Clear();

            int startIndex = m_CurrentPage * k_RecipesPerPage;
            int maxIndex = Math.Min(startIndex + k_RecipesPerPage, Recipes.Count);
            for (int i = startIndex; i < maxIndex; ++i)
            {
                m_DisplayedRecipes.Add(Recipes[i]);
            }

            m_RecipeListView.RefreshItems();
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
            RefreshRecipeList();
        }

        private void OnPageNext()
        {
            if (m_CurrentPage < m_MaxPage)
            {
                ++m_CurrentPage;
            }

            RefreshPageNumber();
            RefreshRecipeList();
        }

        private VisualElement MakeRecipeElement()
        {
            return RecipeVisualAsset.CloneTree();
        }

        private void BindRecipeElement(VisualElement element, int index)
        {
            CraftingRecipeData recipe = m_DisplayedRecipes[index];
            VisualElement icon = element.Q<VisualElement>("ctr_Icon");
            Label itemName = element.Q<Label>("lbl_ItemName");
            Label ingredients = element.Q<Label>("lbl_Ingredients");

            icon.style.backgroundImage = Background.FromSprite(recipe.outputItem.itemIcon);
            itemName.text = recipe.outputItem.itemName;
            ingredients.text = string.Join(", ", recipe.requiredItems.ConvertAll(item => item.itemName));
        }
    }
}
