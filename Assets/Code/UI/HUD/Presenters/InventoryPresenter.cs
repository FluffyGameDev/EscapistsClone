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
        private HUDChannel m_HUDChannel;
        [SerializeField]
        private VisualTreeAsset m_InventorySlot;

        private List<InventorySlot> m_DisplayedSlots;
        private Dictionary<int, InventorySlotPresenter> m_SlotPresenters = new();

        private InventoryActionStrategy m_DefaultStrategy;
        private InventoryActionStrategy m_CurrentStrategy;

        protected override void OnInit()
        {
            m_DefaultStrategy = new DefaultInventoryActionStrategy(m_PlayerChannel, m_InventoryChannel);
            m_CurrentStrategy = m_DefaultStrategy;

            Inventory inventory = m_InventoryHolder.inventory;
            m_DisplayedSlots = new(inventory.slotCount);
            inventory.FilterSlots(slot => true, m_DisplayedSlots);

            view.onSlotViewBound += OnSlotViewBound;
            view.onSlotViewUnbound += OnSlotViewUnbound;

            m_HUDChannel.onRequestInventoryActionStrategy += OnRequestInventoryActionStrategy;

            view.Setup(m_InventorySlot, m_DisplayedSlots);
        }

        protected override void OnShutdown()
        {
            m_HUDChannel.onRequestInventoryActionStrategy -= OnRequestInventoryActionStrategy;
        }

        private void OnSlotViewBound(InventoryView.InventorySlotView view, int index)
        {
            InventorySlotPresenter slotPresenter = new(view, m_DisplayedSlots[index], m_InventoryChannel, m_PlayerChannel);
            m_SlotPresenters[index] = slotPresenter;
            slotPresenter.UpdateActionStrategy(m_CurrentStrategy);
        }

        private void OnSlotViewUnbound(InventoryView.InventorySlotView view, int index)
        {
            if (m_SlotPresenters.Remove(index, out InventorySlotPresenter slotPresenter))
            {
                slotPresenter.Shutdown();
            }
        }

        private void OnRequestInventoryActionStrategy(InventoryActionStrategy newStrategy)
        {
            newStrategy ??= m_DefaultStrategy;
            foreach (var slotPresenter in m_SlotPresenters.Values)
            {
                slotPresenter.UpdateActionStrategy(newStrategy);
            }
        }

        private class InventorySlotPresenter
        {
            private InventoryView.InventorySlotView m_SlotView;
            private InventoryChannel m_InventoryChannel;
            private PlayerChannel m_PlayerChannel;
            private InventoryActionStrategy m_CurrentStrategy;

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
                m_SlotView.onMainAction += OnMainAction;
                m_SlotView.onSecondaryAction += OnSecondaryAction;
            }

            public void Shutdown()
            {
                m_SlotView.onSecondaryAction -= OnSecondaryAction;
                m_SlotView.onMainAction -= OnMainAction;
                m_Slot.OnSlotModified -= UpdateItem;
                m_PlayerChannel.OnToolEquip -= OnToolEquip;
            }

            public void UpdateActionStrategy(InventoryActionStrategy strategy)
            {
                m_CurrentStrategy = strategy;
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

            private void OnMainAction()
            {
                m_CurrentStrategy?.RaiseMainAction(m_Slot, m_ToolBehaviour);
            }

            private void OnSecondaryAction()
            {
                m_CurrentStrategy?.RaiseSecondaryAction(m_Slot, m_ToolBehaviour);
            }
        }
    }
}
