using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Crafting;
using FluffyGameDev.Escapists.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public class CraftingWindowPresenter : HUDElementPresenter<CraftingWindowView>
    {
        [SerializeField]
        private HUDChannel m_HUDChannel;
        [SerializeField]
        private CraftingChannel m_CraftingChannel;
        [SerializeField]
        private UICraftingChannel m_UICraftingChannel;
        [SerializeField]
        private InventoryChannel m_InventoryChannel;

        private InventoryActionStrategy m_CraftingInventoryActionStrategy;

        private Inventory m_CraftingInventory = new();
        private InventorySlot m_OutputSlot;
        private bool m_IsOpen;

        protected override void OnInit()
        {
            m_CraftingInventoryActionStrategy = new CraftingInventoryActionStrategy(m_UICraftingChannel);

            m_CraftingInventory.CreateSlot();
            m_CraftingInventory.CreateSlot();
            m_CraftingInventory.CreateSlot();
            m_OutputSlot = new();

            m_HUDChannel.onRequestCraftingWindow += OnToggleView;

            m_CraftingChannel.onCraftSucceeded += OnCraftingSuccess;
            m_CraftingChannel.onCraftFailed += OnCraftingFailure;
            m_UICraftingChannel.onRequestAddItemToCraft += OnAddItem;

            view.onCraftRequest += OnClickCraft;
            view.onEmptyInputSlotRequest += OnClickInputSlot;
            view.onEmptyOutputSlotRequest += EmptyOutputSlot;
        }

        protected override void OnShutdown()
        {
            view.onEmptyOutputSlotRequest -= EmptyOutputSlot;
            view.onEmptyInputSlotRequest -= OnClickInputSlot;
            view.onCraftRequest -= OnClickCraft;

            m_UICraftingChannel.onRequestAddItemToCraft -= OnAddItem;
            m_CraftingChannel.onCraftFailed -= OnCraftingFailure;
            m_CraftingChannel.onCraftSucceeded -= OnCraftingSuccess;

            m_HUDChannel.onRequestCraftingWindow -= OnToggleView;
        }

        private void OnToggleView()
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                m_HUDChannel?.RaiseRequestInventoryActionStrategy(m_CraftingInventoryActionStrategy);
                view.Display();

                view.UpdateInputSlots(m_CraftingInventory.slots);
                view.UpdateOutputSlot(m_OutputSlot);
            }
            else
            {
                view.Hide();
                m_HUDChannel?.RaiseRequestInventoryActionStrategy(null);

                foreach (var slot in m_CraftingInventory.slots)
                {
                    if (slot.Item != null)
                    {
                        m_InventoryChannel.RaiseItemPickUp(slot.Item);
                        slot.ClearSlot();
                    }
                }

                EmptyOutputSlot();
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            var emtySlot = m_CraftingInventory.FindSlot(slot => slot.Item == null);
            if (emtySlot != null)
            {
                emtySlot.StoreItem(item, 1);
            }

            view.UpdateInputSlots(m_CraftingInventory.slots);
        }

        private void OnClickInputSlot(int slotIndex)
        {
            var slot = m_CraftingInventory.slots[slotIndex];
            if (slot.Item != null)
            {
                m_InventoryChannel.RaiseItemPickUp(slot.Item);
                slot.ClearSlot();
            }

            view.UpdateInputSlots(m_CraftingInventory.slots);
        }

        private void EmptyOutputSlot()
        {
            if (m_OutputSlot.Item != null)
            {
                m_InventoryChannel.RaiseItemPickUp(m_OutputSlot.Item);
                m_OutputSlot.ClearSlot();
            }

            view.UpdateOutputSlot(m_OutputSlot);
        }

        private void OnClickCraft()
        {
            List<InventoryItem> items = new();
            m_CraftingInventory.ForEachSlot(slot =>
            {
                if (slot.Item != null)
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

        private void OnCraftingSuccess(InventoryItem item)
        {
            foreach (var slot in m_CraftingInventory.slots)
            {
                slot.ClearSlot();
            }
            m_OutputSlot.StoreItem(item, 1);

            view.UpdateInputSlots(m_CraftingInventory.slots);
            view.UpdateOutputSlot(m_OutputSlot);
        }

        private void OnCraftingFailure()
        {
            //TODO: show error
        }
    }
}
