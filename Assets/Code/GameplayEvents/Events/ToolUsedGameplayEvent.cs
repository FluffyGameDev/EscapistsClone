using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Gameplay Events/Tool Used")]
    public class ToolUsedGameplayEvent : GameplayEvent<ToolItemBehaviour, TileBase>
    {
    }
}
