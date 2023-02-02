using System;
using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/UI/HUD Channel")]
    public class HUDChannel : ScriptableObject
    {
        public event Action onRequestCraftingWindow;
        public event Action onRequestJournalWindow;
        public event Action<InventoryActionStrategy> onRequestInventoryActionStrategy;

        public void RaiseRequestCraftingWindow()
        {
            onRequestCraftingWindow?.Invoke();
        }

        public void RaiseRequestJournalWindow()
        {
            onRequestJournalWindow?.Invoke();
        }

        public void RaiseRequestInventoryActionStrategy(InventoryActionStrategy strategy)
        {
            onRequestInventoryActionStrategy?.Invoke(strategy);
        }
    }
}
