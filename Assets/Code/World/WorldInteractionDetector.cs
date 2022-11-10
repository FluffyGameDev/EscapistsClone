using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists.World
{
    public class WorldInteractionDetector : MonoBehaviour
    {
        [SerializeField]
        private WorldChannel m_WorldChannel;

        private Tilemap m_TileMap;
        private WorldDataHolder m_WorldDataHolder;

        private void Awake()
        {
            m_TileMap = GetComponent<Tilemap>();
            m_WorldDataHolder = GetComponent<WorldDataHolder>();

            m_WorldChannel.OnWorldInteractionRequest += OnWorldInteractionRequest;
        }

        private void OnDestroy()
        {
            m_WorldChannel.OnWorldInteractionRequest -= OnWorldInteractionRequest;
        }

        private void OnWorldInteractionRequest(Vector3 interactionPosition, Direction direction)
        {
            Vector3Int interactedCellCoordinates = m_TileMap.WorldToCell(interactionPosition);
            interactedCellCoordinates += ComputeDirectionTileOffset(direction);
            TileBase tile = m_TileMap.GetTile(interactedCellCoordinates);
            if (tile != null)
            {
                m_WorldChannel.RaiseWorldInteraction(m_WorldDataHolder, interactedCellCoordinates);
            }
        }

        private Vector3Int ComputeDirectionTileOffset(Direction direction)
        {
            return direction switch
            {
                Direction.Left => Vector3Int.left,
                Direction.Right => Vector3Int.right,
                Direction.Up => Vector3Int.up,
                Direction.Down => Vector3Int.down,
                _ => Vector3Int.zero
            };
        }
    }
}
