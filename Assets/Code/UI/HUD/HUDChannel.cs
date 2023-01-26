using System;
using UnityEngine;

namespace FluffyGameDev.Escapists
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/UI/HUD Channel")]
    public class HUDChannel : ScriptableObject
    {
        public event Action onRequestCraftingWindow;
        public event Action onRequestJournalWindow;

        public void RaiseRequestCraftingWindow()
        {
            onRequestCraftingWindow?.Invoke();
        }

        public void RaiseRequestJournalWindow()
        {
            onRequestJournalWindow?.Invoke();
        }
    }
}
