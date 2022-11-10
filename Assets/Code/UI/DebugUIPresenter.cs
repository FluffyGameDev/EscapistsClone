using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;
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
        [SerializeField]
        private PlayerChannel m_PlayerChannel;

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

            Button dropButton = new();
            dropButton.name = "bt_DropButton";
            dropButton.style.width = 32;
            dropButton.text = "X";
            listElement.Add(dropButton);

            Button useButton = new();
            useButton.name = "bt_EquipButton";
            useButton.text = "Equip";
            listElement.Add(useButton);

            Label label = new();
            label.pickingMode = PickingMode.Ignore;
            listElement.Add(label);

            return listElement;
        }

        private void BindElement(VisualElement element, int index)
        {
            InventorySlot slot = m_DisplayedSlots[index];
            InventorySlotDebugUI slotDebugUI = new(element, slot, m_InventoryChannel, m_PlayerChannel);
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
            private PlayerChannel m_PlayerChannel;
            private InventorySlot m_Slot;
            private Label m_Label;
            private Button m_DropButton;
            private Button m_EquipButton;

            private ToolItemBehaviour m_ToolBehaviour;

            public InventorySlotDebugUI(VisualElement root, InventorySlot slot, InventoryChannel inventoryChannel, PlayerChannel playerChannel)
            {
                m_InventoryChannel = inventoryChannel;
                m_PlayerChannel = playerChannel;
                m_Slot = slot;
                m_Label = root.Q<Label>();
                m_DropButton = root.Q<Button>("bt_DropButton");
                m_EquipButton = root.Q<Button>("bt_EquipButton");

                UpdateItem(slot);
                m_Slot.OnSlotModified += UpdateItem;

                m_DropButton.clicked += OnPressRemoveItem;
                m_EquipButton.clicked += OnPressEquipItem;
            }

            public void Shutdown()
            {
                m_EquipButton.clicked -= OnPressEquipItem;
                m_DropButton.clicked -= OnPressRemoveItem;
                m_Slot.OnSlotModified -= UpdateItem;
            }

            private void UpdateItem(InventorySlot slot)
            {
                bool displaySlot = slot.Item != null;
                m_ToolBehaviour = displaySlot ? slot.Item.FindBehaviour<ToolItemBehaviour>() : null;

                m_EquipButton.style.display = m_ToolBehaviour != null ? DisplayStyle.Flex : DisplayStyle.None;
                m_DropButton.style.display = displaySlot ? DisplayStyle.Flex : DisplayStyle.None;
                m_Label.text = displaySlot ? $"{slot.Item.itemName} x {slot.Quantity}" : "No Item";
            }

            private void OnPressRemoveItem()
            {
                m_InventoryChannel.RaiseItemDrop(m_Slot);
            }

            private void OnPressEquipItem()
            {
                m_PlayerChannel.RaiseToolEquip(m_ToolBehaviour);
            }
        }
    }
}
