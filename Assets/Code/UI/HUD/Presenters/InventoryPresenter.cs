using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class InventoryPresenter : HUDElementPresenter<InventoryView>
    {
        [SerializeField]
        private InventoryHolder m_InventoryHolder;
        [SerializeField]
        private InventoryChannel m_InventoryChannel;
        [SerializeField]
        private PlayerChannel m_PlayerChannel;
        [SerializeField]
        private VisualTreeAsset m_InventorySlot;

        private List<InventorySlot> m_DisplayedSlots;
        private Dictionary<int, InventorySlotPresenter> m_SlotPresenters = new();

        protected override void OnInit()
        {
            Inventory inventory = m_InventoryHolder.inventory;
            m_DisplayedSlots = new(inventory.slotCount);
            inventory.FilterSlots(slot => true, m_DisplayedSlots);

            view.onSlotViewBound += OnSlotViewBound;
            view.onSlotViewUnbound += OnSlotViewUnbound;

            view.Setup(m_InventorySlot, m_DisplayedSlots);
        }

        private void OnSlotViewBound(InventoryView.InventorySlotView view, int index)
        {
            InventorySlotPresenter slotPresenter = new(view, m_DisplayedSlots[index], m_InventoryChannel, m_PlayerChannel);
            m_SlotPresenters[index] = slotPresenter;
        }

        private void OnSlotViewUnbound(InventoryView.InventorySlotView view, int index)
        {
            if (m_SlotPresenters.Remove(index, out InventorySlotPresenter slotPresenter))
            {
                slotPresenter.Shutdown();
            }
        }

        private class InventorySlotPresenter
        {
            private InventoryView.InventorySlotView m_SlotView;
            private InventoryChannel m_InventoryChannel;
            private PlayerChannel m_PlayerChannel;

            private InventorySlot m_Slot;
            private ToolItemBehaviour m_ToolBehaviour;

            public InventorySlotPresenter(InventoryView.InventorySlotView slotView, InventorySlot slot, InventoryChannel inventoryChannel, PlayerChannel playerChannel)
            {
                m_Slot = slot;
                m_SlotView = slotView;
                m_InventoryChannel = inventoryChannel;
                m_PlayerChannel = playerChannel;

                UpdateItem(slot);
                m_Slot.OnSlotModified += UpdateItem;
                m_PlayerChannel.OnToolEquip += OnToolEquip;
                m_SlotView.onEquip += OnEquip;
                m_SlotView.onDrop += OnDrop;
            }

            public void Shutdown()
            {
                m_SlotView.onDrop -= OnDrop;
                m_SlotView.onEquip -= OnEquip;
                m_Slot.OnSlotModified -= UpdateItem;
                m_PlayerChannel.OnToolEquip -= OnToolEquip;
            }

            private void UpdateItem(InventorySlot slot)
            {
                m_ToolBehaviour = slot.Item != null ? slot.Item.FindBehaviour<ToolItemBehaviour>() : null;
                m_SlotView.UpdateItem(slot, m_ToolBehaviour != null);
            }

            private void OnToolEquip(ToolItemBehaviour tool)
            {
                bool isToolEquipped = m_ToolBehaviour != null && tool == m_ToolBehaviour;
                m_SlotView.UpdateEquipState(isToolEquipped);
            }

            private void OnEquip()
            {
                m_PlayerChannel.RaiseToolEquip(m_ToolBehaviour);
            }

            private void OnDrop()
            {
                if (m_ToolBehaviour != null)
                {
                    m_PlayerChannel.RaiseToolEquip(null);
                }
                m_InventoryChannel.RaiseItemDrop(m_Slot);
            }
        }
    }
}
