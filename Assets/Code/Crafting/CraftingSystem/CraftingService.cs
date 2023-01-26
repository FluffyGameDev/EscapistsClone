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
        [SerializeField]
        private CraftingRecipeDatabase m_Database;

        private List<CraftingRecipeData> m_KnownRecipes = new();


        public List<CraftingRecipeData> knownRecipes => m_KnownRecipes;

        private void Awake()
        {
            ServiceLocator.RegisterService<ICraftingService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<ICraftingService>();
        }

        public void Init()
        {
            //TODO
            // Build simplified crafting recipe structures
            // They must have items sorted by ID

            //TODO: access player stats
        }

        public void Shutdown()
        {
        }

        public CraftingRecipeData FindMatchingRecipe(List<InventoryItem> items)
        {
            //TODO: Sort items by ID
            return m_Database.recipes.Find(recipe => DoesRecipeMatch(recipe, items));
        }

        public void TryCraftRecipe(CraftingRecipeData recipe)
        {
            //TODO
            //Check stats
            //  if ok, create the item and send success event
            //  if not, send fail event
        }

        private static bool DoesRecipeMatch(CraftingRecipeData recipe, List<InventoryItem> items)
        {
            // for each item in both, check if equal
            return false;
        }
    }
}
