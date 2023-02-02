using FluffyGameDev.Escapists.InventorySystem;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class CraftingWindowView : HUDElementView
    {
        public event Action onCraftRequest;
        public event Action<int> onEmptyInputSlotRequest;
        public event Action onEmptyOutputSlotRequest;

        private List<VisualElement> m_InputSlots = new();
        private VisualElement m_OutputSlot;
        private VisualElement m_CraftButton;

        public override void Init()
        {
            m_InputSlots.Add(root.Q<VisualElement>("ctr_Slot1"));
            m_InputSlots.Add(root.Q<VisualElement>("ctr_Slot2"));
            m_InputSlots.Add(root.Q<VisualElement>("ctr_Slot3"));
            m_OutputSlot = root.Q<VisualElement>("ctr_SlotResult");
            m_CraftButton = root.Q<VisualElement>("btn_Craft");

            foreach (var slotElement in m_InputSlots)
            {
                slotElement.RegisterCallback<MouseDownEvent>(OnClickInputSlot);
            }
            m_OutputSlot.RegisterCallback<MouseDownEvent>(OnClickOutputSlot);
            m_CraftButton.RegisterCallback<MouseDownEvent>(OnClickCraft);
        }

        public override void Shutdown()
        {
            m_CraftButton.UnregisterCallback<MouseDownEvent>(OnClickCraft);
            m_OutputSlot.UnregisterCallback<MouseDownEvent>(OnClickOutputSlot);
            foreach (var slotElement in m_InputSlots)
            {
                slotElement.UnregisterCallback<MouseDownEvent>(OnClickInputSlot);
            }

            m_InputSlots.Clear();
            m_OutputSlot = null;
            m_CraftButton = null;
        }

        public void UpdateInputSlots(List<InventorySlot> inputSlots)
        {
            for(int i = 0; i < inputSlots.Count; ++i)
            {
                UpdateSlot(m_InputSlots[i], inputSlots[i]);
            }
        }

        public void UpdateOutputSlot(InventorySlot outputSlot)
        {
            UpdateSlot(m_OutputSlot, outputSlot);
        }

        private void UpdateSlot(VisualElement slotElement, InventorySlot slot)
        {
            slotElement.Q<VisualElement>("tx_ItemIcon").style.backgroundImage =
                Background.FromSprite(slot.Item != null ? slot.Item.itemIcon : null);
        }

        private void OnClickInputSlot(MouseDownEvent mouseDownEvent)
        {
            //TODO: change this... It's bad...
            onEmptyInputSlotRequest?.Invoke(m_InputSlots.IndexOf(mouseDownEvent.currentTarget as VisualElement));
        }

        private void OnClickOutputSlot(MouseDownEvent mouseDownEvent)
        {
            onEmptyOutputSlotRequest?.Invoke();
        }

        private void OnClickCraft(MouseDownEvent mouseDownEvent)
        {
            onCraftRequest?.Invoke();
        }
    }
}
