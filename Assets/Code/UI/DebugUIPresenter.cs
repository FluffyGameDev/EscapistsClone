using FluffyGameDev.Escapists.InventorySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    //TODO: remove me ! I'm here for debug purposes only !!!
    public class DebugUIPresenter : MonoBehaviour
    {
        [SerializeField]
        private InventoryHolder m_InventoryHolder;
        [SerializeField]
        private InventoryChannel m_InventoryChannel;

        private ListView m_ListView;
        private List<InventorySlot> m_DisplayedSlots;
        private Dictionary<int, InventorySlotDebugUI> m_DebugUISlots = new();

        private void Start()
        {
            Inventory inventory = m_InventoryHolder.inventory;

            UIDocument uiDocument = GetComponent<UIDocument>();
            m_ListView = uiDocument.rootVisualElement.Q<ListView>();

            m_ListView.makeItem = MakeListElement;
            m_ListView.bindItem = BindElement;
            m_ListView.unbindItem = UnbindElement;

            m_DisplayedSlots = new(inventory.slotCount);
            inventory.FilterSlots(slot => true, m_DisplayedSlots);
            m_ListView.itemsSource = m_DisplayedSlots;
        }

        private VisualElement MakeListElement()
        {
            VisualElement listElement = new();
            listElement.pickingMode = PickingMode.Ignore;
            listElement.style.flexDirection = FlexDirection.Row;

            Button button = new();
            button.style.width = 32;
            button.text = "X";
            listElement.Add(button);

            Label label = new();
            label.pickingMode = PickingMode.Ignore;
            listElement.Add(label);

            return listElement;
        }

        private void BindElement(VisualElement element, int index)
        {
            InventorySlot slot = m_DisplayedSlots[index];
            InventorySlotDebugUI slotDebugUI = new(element, slot, m_InventoryChannel);
            m_DebugUISlots[index] = slotDebugUI;
        }

        private void UnbindElement(VisualElement element, int index)
        {
            if (m_DebugUISlots.Remove(index, out InventorySlotDebugUI slotDebugUI))
            {
                slotDebugUI.Shutdown();
            }
        }


        private class InventorySlotDebugUI
        {
            private InventoryChannel m_InventoryChannel;
            private InventorySlot m_Slot;
            private Label m_Label;
            private Button m_Button;

            public InventorySlotDebugUI(VisualElement root, InventorySlot slot, InventoryChannel inventoryChannel)
            {
                m_InventoryChannel = inventoryChannel;
                m_Slot = slot;
                m_Label = root.Q<Label>();
                m_Button = root.Q<Button>();

                UpdateItemText(slot);
                m_Slot.OnSlotModified += UpdateItemText;

                m_Button.clicked += OnPressRemoveItem;
            }

            public void Shutdown()
            {
                m_Button.clicked -= OnPressRemoveItem;
                m_Slot.OnSlotModified -= UpdateItemText;
            }

            private void UpdateItemText(InventorySlot slot)
            {
                m_Label.text = slot.Item != null ? $"{slot.Item.itemName} x {slot.Quantity}" : "No Item";
            }

            private void OnPressRemoveItem()
            {
                m_InventoryChannel.RaiseItemDrop(m_Slot);
            }
        }
    }
}
