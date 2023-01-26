using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Crafting;
using FluffyGameDev.Escapists.InventorySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class CraftingWindowPresenter : HUDElementPresenter<CraftingWindowView>
    {
        [SerializeField]
        private HUDChannel m_HUDChannel;

        private Inventory m_CraftingInventory = new();
        private InventoryItem m_OutputSlot;
        private bool m_IsOpen;

        protected override void OnInit()
        {
            m_CraftingInventory.CreateSlot();
            m_CraftingInventory.CreateSlot();
            m_CraftingInventory.CreateSlot();

            m_HUDChannel.onRequestCraftingWindow += OnToggleView;

            //TODO: callbacks
        }

        protected override void OnShutdown()
        {
            //TODO: callbacks

            m_HUDChannel.onRequestCraftingWindow -= OnToggleView;
        }

        private void OnToggleView()
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                //TODO: change inventory input strategy
                view.Display();
            }
            else
            {
                view.Hide();
                //TODO: change inventory input strategy
                //TODO: move items back to inventory
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            var emtySlot = m_CraftingInventory.FindSlot(slot => slot.Item == null);
            if (emtySlot != null)
            {
                emtySlot.StoreItem(item, 1);
            }

            //TODO: update view
        }

        private void OnClickInputSlot(int slotIndex)
        {
            //TODO: clear slot
            //TODO: update view
        }

        private void OnClickCraft()
        {
            List<InventoryItem> items = new();
            m_CraftingInventory.ForEachSlot(slot =>
            {
                if (slot.Item == null)
                {
                    items.Add(slot.Item);
                }
            });

            ICraftingService craftingService = ServiceLocator.LocateService<ICraftingService>();
            CraftingRecipeData recipe = craftingService.FindMatchingRecipe(items);
            if (recipe != null)
            {
                craftingService.TryCraftRecipe(recipe);
            }
            else
            {
                //TODO: show error
            }
        }

        private void OnCraftingSuccess()
        {
            // m_OutputSlot = ???
            // Update view
        }

        private void OnCraftingFailure()
        {
            //TODO: show error
        }
    }
}
