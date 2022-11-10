using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Player/Player Channel")]
    public class PlayerChannel : ScriptableObject
    {
        public delegate void ToolEquipCallback(ToolItemBehaviour tool);

        public event ToolEquipCallback OnToolEquip;

        public void RaiseToolEquip(ToolItemBehaviour tool)
        {
            OnToolEquip?.Invoke(tool);
        }
    }
}
