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
        [SerializeField]
        private VisualTreeAsset m_InventorySlot;

        [SerializeField]
        private Stats.StatHolder m_StatHolder;
        [SerializeField]
        private Stats.StatDescriptor m_HealthStat;
        [SerializeField]
        private Stats.StatDescriptor m_StaminaStat;
        [SerializeField]
        private Stats.StatDescriptor m_HeatStat;

        [SerializeField]
        private PlayerStateMachineHolder m_PlayerStateMachineHolder;

        private Label m_TimeLabel;
        private Label m_ActivityLabel;
        private ListView m_ListView;
        private VisualElement m_ToolProgressContainer;
        private ProgressBar m_ToolProgressBar;
        private List<InventorySlot> m_DisplayedSlots;
        private Dictionary<int, InventorySlotDebugUI> m_DebugUISlots = new();

        private List<StatUIPresenter> m_StatPresenters = new();

        private void Start()
        {
            Inventory inventory = m_InventoryHolder.inventory;

            UIDocument uiDocument = GetComponent<UIDocument>();
            m_ListView = uiDocument.rootVisualElement.Q<ListView>();
            m_TimeLabel = uiDocument.rootVisualElement.Q<Label>("lbl_Time");
            m_ActivityLabel = uiDocument.rootVisualElement.Q<Label>("lbl_Activity");
            m_ToolProgressContainer = uiDocument.rootVisualElement.Q<VisualElement>("ctr_tool");
            m_ToolProgressBar = uiDocument.rootVisualElement.Q<ProgressBar>("pb_ToolProgress");

            m_ListView.makeItem = MakeListElement;
            m_ListView.bindItem = BindElement;
            m_ListView.unbindItem = UnbindElement;

            m_DisplayedSlots = new(inventory.slotCount);
            inventory.FilterSlots(slot => true, m_DisplayedSlots);
            m_ListView.itemsSource = m_DisplayedSlots;

            ServiceLocator.WaitUntilReady<ITimeService>(InitTimeUI);
            ServiceLocator.WaitUntilReady<IScheduleService>(InitActivityUI);

            m_StatPresenters.Add(new StatUIPresenter(uiDocument.rootVisualElement.Q<Label>("lbl_HealthValue"), m_StatHolder.Stats, m_HealthStat));
            m_StatPresenters.Add(new StatUIPresenter(uiDocument.rootVisualElement.Q<Label>("lbl_StaminaValue"), m_StatHolder.Stats, m_StaminaStat));
            m_StatPresenters.Add(new StatUIPresenter(uiDocument.rootVisualElement.Q<Label>("lbl_HeatValue"), m_StatHolder.Stats, m_HeatStat));
        }

        private void OnDestroy()
        {
            foreach (StatUIPresenter presenter in m_StatPresenters)
            {
                presenter.Shutdown();
            }
            m_StatPresenters.Clear();
        }

        private void Update()
        {
            UpdateToolUseBar();
        }

        private void InitTimeUI()
        {
            ITimeService timeService = ServiceLocator.LocateService<ITimeService>();
            DateTime time = timeService.CurrentTime;
            m_TimeLabel.text = $"Day {time.Day}: {time.Hour:D2}:{time.Minute:D2}";
            timeService.OnTimeChanges += OnTimeChange;
        }

        private void InitActivityUI()
        {
            IScheduleService scheduleService = ServiceLocator.LocateService<IScheduleService>();
            m_ActivityLabel.text = scheduleService.CurrentActivity != null ? scheduleService.CurrentActivity.ActivityName : "No Activity";
            scheduleService.OnActivityChange += OnActivityChange;
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
            VisualElement listElement = m_InventorySlot.CloneTree();
            listElement.style.flexDirection = FlexDirection.Row;
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

        private void UpdateToolUseBar()
        {
            bool isUsingTool = m_PlayerStateMachineHolder.blackboard.Get<bool>((int)PlayerBB.IsUsingTool);
            m_ToolProgressContainer.style.display = isUsingTool ? DisplayStyle.Flex : DisplayStyle.None;

            if (isUsingTool)
            {
                float startTime = m_PlayerStateMachineHolder.blackboard.Get<float>((int)PlayerBB.ToolUseStartTime);
                float endTime = m_PlayerStateMachineHolder.blackboard.Get<float>((int)PlayerBB.ToolUseEndTime);

                float duration = endTime - startTime;
                float progress = (Time.time - startTime) / duration;

                m_ToolProgressBar.value = progress;
            }
        }


        private class InventorySlotDebugUI
        {
            private InventoryChannel m_InventoryChannel;
            private PlayerChannel m_PlayerChannel;
            private InventorySlot m_Slot;
            private VisualElement m_Root;
            private VisualElement m_Icon;

            private ToolItemBehaviour m_ToolBehaviour;

            public InventorySlotDebugUI(VisualElement root, InventorySlot slot, InventoryChannel inventoryChannel, PlayerChannel playerChannel)
            {
                m_InventoryChannel = inventoryChannel;
                m_PlayerChannel = playerChannel;
                m_Slot = slot;

                m_Root = root;
                m_Icon = m_Root.Q<VisualElement>("tx_ItemIcon");

                m_Root.RegisterCallback<MouseDownEvent>(OnSlotClick);

                UpdateItem(slot);
                m_Slot.OnSlotModified += UpdateItem;

                m_PlayerChannel.OnToolEquip += OnToolEquip;
            }

            public void Shutdown()
            {
                m_Root.UnregisterCallback<MouseDownEvent>(OnSlotClick);

                m_PlayerChannel.OnToolEquip -= OnToolEquip;
                m_Slot.OnSlotModified -= UpdateItem;
            }

            private void UpdateItem(InventorySlot slot)
            {
                bool displaySlot = slot.Item != null;
                m_ToolBehaviour = displaySlot ? slot.Item.FindBehaviour<ToolItemBehaviour>() : null;

                if (m_ToolBehaviour == null)
                {
                    m_Root.Q("ctr_slot").RemoveFromClassList("selectedItem");
                }

                m_Icon.style.backgroundImage = Background.FromSprite(displaySlot ? slot.Item.itemIcon : null);
            }

            private void OnToolEquip(ToolItemBehaviour tool)
            {
                bool isToolEquipped = m_ToolBehaviour != null && tool == m_ToolBehaviour;
                m_Root.Q("ctr_slot").EnableInClassList("selectedItem", isToolEquipped);
            }

            private void OnSlotClick(MouseDownEvent mouseDownEvent)
            {
                switch (mouseDownEvent.button)
                {
                    case 0:
                    {
                        m_PlayerChannel.RaiseToolEquip(m_ToolBehaviour);
                        break;
                    }

                    case 1:
                    {
                        if (m_ToolBehaviour != null)
                        {
                            m_PlayerChannel.RaiseToolEquip(null);
                        }
                        m_InventoryChannel.RaiseItemDrop(m_Slot);
                        break;
                    }
                }

            }
        }

        private class StatUIPresenter
        {
            private Label m_ValueLabel;
            private Stats.Stat m_Stat;

            public StatUIPresenter(Label valueLabel, Stats.StatsContainer statsContainer, Stats.StatData statDescriptor)
            {
                m_ValueLabel = valueLabel;
                m_Stat = statsContainer.GetStat(statDescriptor);

                RefreshStatValue(m_Stat);
                m_Stat.OnStatChanged += RefreshStatValue;
            }

            public void Shutdown()
            {
                m_Stat.OnStatChanged -= RefreshStatValue;
            }

            private void RefreshStatValue(Stats.Stat stat)
            {
                switch (stat.StatType)
                {
                    case Stats.StatType.Integer:
                    {
                        m_ValueLabel.text = stat.GetValueInt().ToString();
                        break;
                    }

                    case Stats.StatType.Float:
                    {
                        m_ValueLabel.text = $"{(int)(stat.GetValueFloat() * 100.0f)} %";
                        break;
                    }
                }
            }
        }
    }
}
