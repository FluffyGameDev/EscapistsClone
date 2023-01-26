using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Crafting
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Crafting/Crafting Recipe Database")]
    public class CraftingRecipeDatabase : ScriptableObject
    {
        [SerializeField]
        private List<CraftingRecipeData> m_Recipes;

        public List<CraftingRecipeData> recipes => m_Recipes;
    }
}