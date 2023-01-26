using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class CraftingWindowView : HUDElementView
    {
        public event Action onCraftRequest;
        public event Action<int> onEmptySlotRequest;

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

            //TODO: callbacks
        }

        public override void Shutdown()
        {
            //TODO: callbacks

            m_InputSlots.Clear();
            m_OutputSlot = null;
            m_CraftButton = null;
        }

        private void UpdateInputSlots()
        {
            //TODO
        }

        private void UpdateOutputSlot()
        {
            //TODO
        }

        private void OnClickInputSlot(MouseDownEvent mouseDownEvent)
        {
            //TODO: change this... It's bad...
            onEmptySlotRequest?.Invoke(m_InputSlots.IndexOf(mouseDownEvent.currentTarget as VisualElement));
        }

        private void OnClickCraft(MouseDownEvent mouseDownEvent)
        {
            onCraftRequest?.Invoke();
        }
    }
}
