#if UNITY_EDITOR

using UnityEngine;
using Unity.Entities;
using UnityEditor;
using Unity.Mathematics;

namespace FluffyGameDev.Escapists.AI.Debug
{
    public class WorldDebugView : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            EntityManager entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery query = new EntityQueryBuilder(Unity.Collections.Allocator.Temp).WithAll<WorldDataComponent>().Build(entityManager);

            if (query.HasSingleton<WorldDataComponent>())
            {
                //TODO: change to avoid copy
                WorldDataComponent worldData = query.GetSingleton<WorldDataComponent>();

                Camera editorCamera = SceneView.lastActiveSceneView.camera;
                Rect viewport = SceneView.lastActiveSceneView.cameraViewport;
                Vector3 viewMin = editorCamera.ScreenToWorldPoint(viewport.min);
                Vector3 viewMax = editorCamera.ScreenToWorldPoint(viewport.max);

                int2 worldGridMin = worldData.WorldToCell(viewMin);
                int2 worldGridMax = worldData.WorldToCell(viewMax);

                Gizmos.color = Color.red;
                Vector3 cellSize = new Vector3(worldData.CellSize.x, worldData.CellSize.y, 1.0f);
                for (int y = worldGridMin.y; y <= worldGridMax.y; ++y)
                {
                    for (int x = worldGridMin.x; x <= worldGridMax.x; ++x)
                    {
                        int2 currentCell = new int2(x, y);
                        if (worldData.IsInBounds(currentCell))
                        {
                            int cellIndex = worldData.CellToIndex(currentCell);
                            bool isCollider = worldData.Cells[cellIndex].IsCellCollider;
                            Vector3 cellPosition = worldData.CellToWorld(currentCell);
                            if (isCollider)
                            {
                                Gizmos.DrawWireCube(cellPosition, cellSize);
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif
