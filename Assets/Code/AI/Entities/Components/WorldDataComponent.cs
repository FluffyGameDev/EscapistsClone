using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace FluffyGameDev.Escapists.AI
{
    public struct WorldCellData
    {
        public bool IsCellCollider;
    }

    public struct WorldDataComponent : IComponentData
    {
        public NativeArray<WorldCellData> Cells;
        public int2 WorldSize;
        public float2 WorldBoundMin;
        public float2 WorldBoundSize;
        public float2 CellSize;

        public bool IsInBounds(int2 cellPosition)
        {
            return cellPosition.x >= 0 && cellPosition.x < WorldSize.x && cellPosition.y >= 0 && cellPosition.y < WorldSize.y;
        }

        public float3 CellToWorld(int2 cellPosition)
        {
            return new float3((cellPosition.x + 0.5f) * CellSize.x + WorldBoundMin.x,
                              (cellPosition.y + 0.5f) * CellSize.y + WorldBoundMin.y,
                              0.0f);
        }

        public int2 WorldToCell(float3 worldPosition)
        {
            return new int2((int)((worldPosition.x - WorldBoundMin.x) / CellSize.x),
                            (int)((worldPosition.y - WorldBoundMin.y) / CellSize.y));
        }

        public int CellToIndex(int2 cellPosition)
        {
            return cellPosition.x + cellPosition.y * WorldSize.x;
        }
    }
}
