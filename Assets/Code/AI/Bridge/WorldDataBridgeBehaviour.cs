using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists.AI
{
    public class WorldDataBridgeBehaviour : MonoBehaviour
    {
        private static WorldDataBridgeBehaviour s_Instance;
        public static WorldDataBridgeBehaviour Instance => s_Instance;

        private Tilemap m_Tilemap;

        void Awake()
        {
            //TODO: turn into service
            s_Instance = this;
            m_Tilemap = GetComponent<Tilemap>();
        }

        public Vector3Int WorldSize => m_Tilemap.size;
        public Bounds WorldBound => m_Tilemap.localBounds;
        public Vector3 CellSize => m_Tilemap.cellSize;

        public bool IsTileCollider(int x, int y) => m_Tilemap.GetColliderType(new Vector3Int(x, y, 0) + m_Tilemap.cellBounds.position) != Tile.ColliderType.None;
    }
}
