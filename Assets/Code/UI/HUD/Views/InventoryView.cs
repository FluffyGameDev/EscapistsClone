using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class InventoryView : HUDElementView
    {
        private VisualTreeAsset m_InventorySlot;
        private ListView m_ListView;
        private Dictionary<int, InventorySlotView> m_SlotViews = new();

        public event Action<InventorySlotView, int> onSlotViewBound;
        public event Action<InventorySlotView, int> onSlotViewUnbound;

        public void Setup(VisualTreeAsset inventorySlot, List<InventorySlot> displayedSlots)
        {
            m_InventorySlot = inventorySlot;

            m_ListView = root.Q<ListView>();
            m_ListView.makeItem = MakeListElement;
            m_ListView.bindItem = BindElement;
            m_ListView.unbindItem = UnbindElement;

            m_ListView.itemsSource = displayedSlots;
        }

        private VisualElement MakeListElement()
        {
            VisualElement listElement = m_InventorySlot.CloneTree();
            listElement.style.flexDirection = FlexDirection.Row;
            return listElement;
        }

        private void BindElement(VisualElement element, int index)
        {
            InventorySlotView slotView = new(element);
            m_SlotViews[index] = slotView;
            onSlotViewBound?.Invoke(slotView, index);
        }

        private void UnbindElement(VisualElement element, int index)
        {
            if (m_SlotViews.Remove(index, out InventorySlotView slotView))
            {
                slotView.Shutdown();
                onSlotViewUnbound?.Invoke(slotView, index);
            }
        }

        public class InventorySlotView
        {
            private VisualElement m_Root;
            private VisualElement m_Icon;

            public event Action onMainAction;
            public event Action onSecondaryAction;

            public InventorySlotView(VisualElement root)
            {
                m_Root = root;
                m_Icon = m_Root.Q<VisualElement>("tx_ItemIcon");

                m_Root.RegisterCallback<MouseDownEvent>(OnSlotClick);
            }

            public void Shutdown()
            {
                m_Root.UnregisterCallback<MouseDownEvent>(OnSlotClick);
            }

            public void UpdateEquipState(bool isToolEquipped)
            {
                m_Root.Q("ctr_slot").EnableInClassList("selectedItem", isToolEquipped);
            }

            public void UpdateItem(InventorySlot slot, bool isToolEquipped)
            {
                bool displaySlot = slot.Item != null;

                if (!isToolEquipped)
                {
                    m_Root.Q("ctr_slot").RemoveFromClassList("selectedItem");
                }

                m_Icon.style.backgroundImage = Background.FromSprite(displaySlot ? slot.Item.itemIcon : null);
            }

            private void OnSlotClick(MouseDownEvent mouseDownEvent)
            {
                switch (mouseDownEvent.button)
                {
                    case 0:
                    {
                        onMainAction?.Invoke();
                        break;
                    }

                    case 1:
                    {
                        onSecondaryAction?.Invoke();
                        break;
                    }
                }

            }
        }
    }
}
