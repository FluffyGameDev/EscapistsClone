using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists
{
    public class WorldDataHolder : MonoBehaviour
    {
        private struct CellData
        {
            public float damage;
        }

        [SerializeField]
        private TileBase m_DestroyedWallTile;

        private Tilemap m_TileMap;

        private CellData[] m_CellsData;
        private BoundsInt m_TilemapBounds;


        private void Awake()
        {
            m_TileMap = GetComponent<Tilemap>();

            m_TilemapBounds = m_TileMap.cellBounds;
            int cellCount = m_TilemapBounds.size.x * m_TilemapBounds.size.y;
            m_CellsData = new CellData[cellCount];
        }

        public void DamageTile(Vector3Int position, float damageAmount)
        {
            //TODO: use unsafe code to access pointer ?
            Vector3Int adjustedPosition = position - m_TilemapBounds.min;
            int cellIndex = adjustedPosition.x + adjustedPosition.y * m_TilemapBounds.size.x;

            CellData cell = m_CellsData[cellIndex];
            cell.damage += damageAmount;

            if (cell.damage >= 1)
            {
                m_TileMap.SetTile(position, m_DestroyedWallTile);
            }

            m_CellsData[cellIndex] = cell;
        }

        public TileBase GetTileAtPosition(Vector3Int position)
        {
            return m_TileMap.GetTile(position);
        }
    }
}
