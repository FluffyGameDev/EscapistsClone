using UnityEngine;

namespace FluffyGameDev.Escapists.World
{
    //TODO: Move elsewhere
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/World/World Channel")]
    public class WorldChannel : ScriptableObject
    {
        public delegate void WorldInteractionRequestCallback(Vector3 interactor, Direction direction);
        public delegate void WorldInteractionCallback(WorldDataHolder worldDataHolder, Vector3Int interactionPosition);

        public event WorldInteractionRequestCallback OnWorldInteractionRequest;
        public event WorldInteractionCallback OnWorldInteraction;

        public void RaiseWorldInteractionRequest(Vector3 interactor, Direction direction)
        {
            OnWorldInteractionRequest?.Invoke(interactor, direction);
        }

        public void RaiseWorldInteraction(WorldDataHolder worldDataHolder, Vector3Int interactionPosition)
        {
            OnWorldInteraction?.Invoke(worldDataHolder, interactionPosition);
        }
    }
}
