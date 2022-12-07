using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;
using FluffyGameDev.Escapists.World;
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

        private Label m_TimeLabel;
        private Label m_ActivityLabel;
        private ListView m_ListView;
        private List<InventorySlot> m_DisplayedSlots;
        private Dictionary<int, InventorySlotDebugUI> m_DebugUISlots = new();

        private void Start()
        {
            Inventory inventory = m_InventoryHolder.inventory;

            UIDocument uiDocument = GetComponent<UIDocument>();
            m_ListView = uiDocument.rootVisualElement.Q<ListView>();
            m_TimeLabel = uiDocument.rootVisualElement.Q<Label>("lbl_Time");
            m_ActivityLabel = uiDocument.rootVisualElement.Q<Label>("lbl_Activity");

            m_ListView.makeItem = MakeListElement;
            m_ListView.bindItem = BindElement;
            m_ListView.unbindItem = UnbindElement;

            m_DisplayedSlots = new(inventory.slotCount);
            inventory.FilterSlots(slot => true, m_DisplayedSlots);
            m_ListView.itemsSource = m_DisplayedSlots;

            ITimeService timeService = ServiceLocator.LocateService<ITimeService>();
            DateTime time = timeService.CurrentTime;
            m_TimeLabel.text = $"Day {time.Day}: {time.Hour:D2}:{time.Minute:D2}";
            timeService.OnTimeChanges += OnTimeChange;

            IScheduleService scheduleService = ServiceLocator.LocateService<IScheduleService>();
            m_ActivityLabel.text = scheduleService.CurrentActivity != null ? scheduleService.CurrentActivity.ActivityName : "No Activity";
            scheduleService.OnActivityChange += OnActivityChange;
        }

        private void OnDestroy()
        {
            ITimeService timeService = ServiceLocator.LocateService<ITimeService>();
            timeService.OnTimeChanges -= OnTimeChange;

            IScheduleService scheduleService = ServiceLocator.LocateService<IScheduleService>();
            scheduleService.OnActivityChange -= OnActivityChange;
        }

        private void OnTimeChange(DateTime time)
        {
            m_TimeLabel.text = $"Day {time.Day}: {time.Hour:D2}:{time.Minute:D2}";
        }

        private void OnActivityChange(Activity activity)
        {
            m_ActivityLabel.text = activity != null ? activity.ActivityName : "No Activity";
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
                if (m_ToolBehaviour != null)
                {
                    m_PlayerChannel.RaiseToolEquip(null);
                }
                m_InventoryChannel.RaiseItemDrop(m_Slot);
            }

            private void OnPressEquipItem()
            {
                m_PlayerChannel.RaiseToolEquip(m_ToolBehaviour);
            }
        }
    }
}
