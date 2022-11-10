using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists
{
    public class WorldDataHolder : MonoBehaviour
    {
        private Tilemap m_TileMap;

        private void Awake()
        {
            m_TileMap = GetComponent<Tilemap>();
        }

        public void DamageTile(Vector3Int position, float damageAmount)
        {
            //TODO: apply damage
            m_TileMap.SetTile(position, null);
        }
    }
}
