using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Player/Player Channel")]
    public class PlayerChannel : ScriptableObject
    {
        public delegate void ToolEquipCallback(ToolItemBehaviour tool);
        public delegate void ToolUseSucceededCallback();

        public event ToolEquipCallback OnToolEquip;
        public event ToolUseSucceededCallback OnToolUseSucceeded;

        public void RaiseToolEquip(ToolItemBehaviour tool)
        {
            OnToolEquip?.Invoke(tool);
        }

        public void RaiseToolUseSucceeded()
        {
            OnToolUseSucceeded?.Invoke();
        }
    }
}
