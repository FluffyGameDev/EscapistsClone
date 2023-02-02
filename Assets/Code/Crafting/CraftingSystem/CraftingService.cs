using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Crafting
{
    public interface ICraftingService : IService
    {
        List<CraftingRecipeData> knownRecipes { get; }
        CraftingRecipeData FindMatchingRecipe(List<InventoryItem> items);
        void TryCraftRecipe(CraftingRecipeData recipe);
    }

    public class CraftingService : MonoBehaviour, ICraftingService
    {
        private class CraftingRecipe
        {
            public CraftingRecipeData dataSource;
            public List<int> itemIDs = new();
            public int requiredIntelligence;
        }

        [SerializeField]
        private CraftingRecipeDatabase m_Database;

        [SerializeField]
        private CraftingChannel m_CraftingChannel;

        private List<CraftingRecipe> m_AllRecipes = new();
        private List<CraftingRecipeData> m_KnownRecipes = new();

        public List<CraftingRecipeData> knownRecipes => m_KnownRecipes;

        private void Start()
        {
            ServiceLocator.RegisterService<ICraftingService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<ICraftingService>();
        }

        public void Init()
        {
            BakeRecipes();

            //TODO: access player stats
        }

        public void Shutdown()
        {
        }

        public CraftingRecipeData FindMatchingRecipe(List<InventoryItem> items)
        {
            List<int> itemIDs = new();
            foreach (var item in items)
            {
                if (item!= null)
                {
                    itemIDs.Add(item.itemID);
                }
            }
            itemIDs.Sort();

            CraftingRecipe foundRecipe = m_AllRecipes.Find(recipe => DoesRecipeMatch(recipe, itemIDs));
            return foundRecipe != null ? foundRecipe.dataSource : null;
        }

        public void TryCraftRecipe(CraftingRecipeData recipe)
        {
            if (true) //TODO: chack stats
            {
                InventoryItem createdItem = recipe.outputItem.CreateItem();
                m_CraftingChannel.RaiseCraftSucceeded(createdItem);
            }
            else
            {
                m_CraftingChannel.RaiseCraftFailed();
            }
        }

        private static bool DoesRecipeMatch(CraftingRecipe recipe, List<int> itemIDs)
        {
            if (recipe.itemIDs.Count == itemIDs.Count)
            {
                for (int i = 0; i < recipe.itemIDs.Count; ++i)
                {
                    if (itemIDs[i] != recipe.itemIDs[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void BakeRecipes()
        {
            m_AllRecipes = m_Database.recipes.ConvertAll(recipe => BakeRecipe(recipe));
        }

        private CraftingRecipe BakeRecipe(CraftingRecipeData data)
        {
            CraftingRecipe craftingRecipe = new();

            foreach (var item in data.requiredItems)
            {
                craftingRecipe.itemIDs.Add(item.itemID);
            }
            craftingRecipe.itemIDs.Sort();

            craftingRecipe.dataSource = data;
            craftingRecipe.requiredIntelligence = data.requiredIntelligence;
            return craftingRecipe;
        }
    }
}
