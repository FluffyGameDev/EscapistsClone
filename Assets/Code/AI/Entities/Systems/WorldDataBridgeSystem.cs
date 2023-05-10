using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FluffyGameDev.Escapists.AI
{
    public partial struct WorldDataBridgeSystem : ISystem
    {
        private bool m_IsInit;

        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.CreateSingleton<WorldDataComponent>();
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!m_IsInit && WorldDataBridgeBehaviour.Instance != null)
            {
                WorldDataBridgeBehaviour worldData = WorldDataBridgeBehaviour.Instance;
                int2 worldSize = new int2(worldData.WorldSize.x, worldData.WorldSize.y);
                NativeArray<WorldCellData> worldCells = new NativeArray<WorldCellData>(worldSize.x * worldSize.y, Allocator.Persistent);
                for (int j = 0; j < worldSize.y; ++j)
                {
                    for (int i = 0; i < worldSize.x; ++i)
                    {
                        bool isCollider = worldData.IsTileCollider(i, j);
                        worldCells[i + j * worldSize.x] = new WorldCellData()
                        {
                            IsCellCollider = isCollider
                        };
                    }
                }

                Bounds bounds = worldData.WorldBound;
                SystemAPI.SetSingleton(new WorldDataComponent()
                {
                    Cells = worldCells,
                    WorldSize = worldSize,
                    WorldBoundMin = new float2(bounds.min.x, bounds.min.y),
                    WorldBoundSize = new float2(bounds.size.x, bounds.size.y),
                    CellSize = new float2(worldData.CellSize.x, worldData.CellSize.y)
                });

                m_IsInit = true;
            }
        }
    }
}
