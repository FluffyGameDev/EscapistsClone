using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FluffyGameDev.Escapists.AI
{
    struct PathFindingWorkData
    {
        public int CellDistance;
    }

    //[BurstCompile]
    public partial struct PathFindingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            WorldDataComponent worldData = SystemAPI.GetSingleton<WorldDataComponent>();

            //TODO: move to a job
            foreach (var (pathFinding, movementTarget, worldTransform) in SystemAPI.Query<RefRW<PathFindingComponent>, RefRW<MovementTargetComponent>, WorldTransform>())
            {
                switch (pathFinding.ValueRO.State)
                {
                    case PathFindingState.Searching:
                    {
                        FindPath(ref pathFinding.ValueRW, worldTransform, ref worldData);
                        break;
                    }
                    case PathFindingState.Moving:
                    {
                        FollowPath(ref pathFinding.ValueRW, ref movementTarget.ValueRW, ref worldData);
                        break;
                    }
                }
            }
        }

        private void FindPath(ref PathFindingComponent pathFinding, WorldTransform worldTransform, ref WorldDataComponent worldData)
        {
            int2 startCell = worldData.WorldToCell(worldTransform.Position);
            int2 endCell = worldData.WorldToCell(pathFinding.TargetPosition);

            if (startCell.x == endCell.x && startCell.y == endCell.y)
            {
                pathFinding.State = PathFindingState.AtDestination;
                return;
            }

            NativeArray<PathFindingWorkData> workData = new NativeArray<PathFindingWorkData>(worldData.Cells.Length, Allocator.Temp);
            NativeList<int2> nextCheckPositions = new NativeList<int2>(worldData.WorldSize.y, Allocator.Temp);

            for (int i = 0; i < workData.Length; ++i)
            {
                workData[i] = new PathFindingWorkData() { CellDistance = -1 };
            }

            nextCheckPositions.Add(startCell);
            int startCellIndex = worldData.CellToIndex(startCell);
            workData[startCellIndex] = new PathFindingWorkData() { CellDistance = 0 };

            while (nextCheckPositions.Length > 0)
            {
                int2 cellPosition = nextCheckPositions[0];
                int cellIndex = worldData.CellToIndex(cellPosition);
                int cellDistance = workData[cellIndex].CellDistance;
                nextCheckPositions.RemoveAt(0);

                InspectNeighbor(new int2(cellPosition.x - 1, cellPosition.y), endCell, cellDistance, ref workData, ref nextCheckPositions, ref worldData);
                InspectNeighbor(new int2(cellPosition.x + 1, cellPosition.y), endCell, cellDistance, ref workData, ref nextCheckPositions, ref worldData);
                InspectNeighbor(new int2(cellPosition.x, cellPosition.y - 1), endCell, cellDistance, ref workData, ref nextCheckPositions, ref worldData);
                InspectNeighbor(new int2(cellPosition.x, cellPosition.y + 1), endCell, cellDistance, ref workData, ref nextCheckPositions, ref worldData);
            }

            ComputePath(endCell, ref workData, ref worldData, ref pathFinding.Path);

            pathFinding.State = PathFindingState.Moving;
        }

        private void InspectNeighbor(int2 cellPosition, int2 endCell, int previousDistance, ref NativeArray<PathFindingWorkData> workData, ref NativeList<int2> nextCheckPositions, ref WorldDataComponent worldData)
        {
            if (cellPosition.x == endCell.x && cellPosition.y == endCell.y)
            {
                int cellIndex = worldData.CellToIndex(cellPosition);
                workData[cellIndex] = new PathFindingWorkData() { CellDistance = previousDistance + 1 };
                nextCheckPositions.Clear();
            }
            else if (worldData.IsInBounds(cellPosition))
            {
                int cellIndex = worldData.CellToIndex(cellPosition);
                if (workData[cellIndex].CellDistance == -1 && !worldData.Cells[cellIndex].IsCellCollider)
                {
                    InsertPositionToInspect(cellPosition, endCell, ref nextCheckPositions);
                    workData[cellIndex] = new PathFindingWorkData() { CellDistance = previousDistance + 1 };
                }
            }
        }

        private void InsertPositionToInspect(int2 cellPosition, int2 endCell, ref NativeList<int2> nextCheckPositions)
        {
            //TODO: binary search
            int insertPosition = 0;
            while (insertPosition < nextCheckPositions.Length && !CanInsertPosition(cellPosition, insertPosition, endCell, ref nextCheckPositions))
            {
                ++insertPosition;
            }

            nextCheckPositions.InsertRange(insertPosition, 1);
            nextCheckPositions[insertPosition] = cellPosition;
        }

        private bool CanInsertPosition(int2 cellPosition, int insertPosition, int2 endCell, ref NativeList<int2> nextCheckPositions)
        {
            int2 currentDiff = math.abs(cellPosition - endCell);
            int2 insertPositionDiff = math.abs(nextCheckPositions[insertPosition] - endCell);
            return currentDiff.x + currentDiff.y < insertPositionDiff.x + insertPositionDiff.y;
        }

        private void ComputePath(int2 endCell, ref NativeArray<PathFindingWorkData> workData, ref WorldDataComponent worldData, ref NativeList<int2> path)
        {
            path.Clear();

            int2 currentCell = endCell;
            int currentCellIndex = worldData.CellToIndex(currentCell);
            int currentDistance = workData[currentCellIndex].CellDistance;

            path.Add(currentCell);
            while (currentDistance > 0)
            {
                --currentDistance;
                FindNextCell(ref currentCell, ref workData, ref worldData, currentDistance);

                bool overridePrevious = false;
                if (path.Length > 1)
                {
                    int2 previousDiff = path[path.Length - 1] - path[path.Length - 2];
                    int2 currentDiff = currentCell - path[path.Length - 1];

                    int2 previousSign = new int2(
                        (previousDiff.x > 0 ? 1 : (previousDiff.x < 0 ? -1 : 0)),
                        (previousDiff.y > 0 ? 1 : (previousDiff.y < 0 ? -1 : 0)));
                    int2 currentSign = new int2(
                        (currentDiff.x > 0 ? 1 : (currentDiff.x < 0 ? -1 : 0)),
                        (currentDiff.y > 0 ? 1 : (currentDiff.y < 0 ? -1 : 0)));

                    overridePrevious = (currentSign.x == previousSign.x && currentSign.y == previousSign.y);
                }

                if (overridePrevious)
                {
                    path[path.Length - 1] = currentCell;
                }
                else
                {
                    path.Add(currentCell);
                }
            }

            for (int i = 0; i < path.Length / 2; ++i)
            {
                int2 temp = path[i];
                path[i] = path[path.Length - i - 1];
                path[path.Length - i - 1] = temp;
            }
        }

        private void FindNextCell(ref int2 currentCell, ref NativeArray<PathFindingWorkData> workData, ref WorldDataComponent worldData, int currentDistance)
        {
            if (currentCell.x > 0 && workData[worldData.CellToIndex(new int2(currentCell.x - 1, currentCell.y))].CellDistance == currentDistance)
            {
                currentCell = new int2(currentCell.x - 1, currentCell.y);
            }
            else if (currentCell.x < worldData.WorldSize.x - 1 && workData[worldData.CellToIndex(new int2(currentCell.x + 1, currentCell.y))].CellDistance == currentDistance)
            {
                currentCell = new int2(currentCell.x + 1, currentCell.y);
            }
            else if (currentCell.y > 0 && workData[worldData.CellToIndex(new int2(currentCell.x, currentCell.y - 1))].CellDistance == currentDistance)
            {
                currentCell = new int2(currentCell.x, currentCell.y - 1);
            }
            else if (currentCell.y < worldData.WorldSize.y - 1 && workData[worldData.CellToIndex(new int2(currentCell.x, currentCell.y + 1))].CellDistance == currentDistance)
            {
                currentCell = new int2(currentCell.x, currentCell.y + 1);
            }
        }

        private void FollowPath(ref PathFindingComponent pathFinding, ref MovementTargetComponent movementTarget, ref WorldDataComponent worldData)
        {
            if (!movementTarget.IsActive)
            {
                ++pathFinding.CurrentStepIndex;

                if (pathFinding.CurrentStepIndex < pathFinding.Path.Length)
                {
                    movementTarget.TargetPosition = worldData.CellToWorld(pathFinding.Path[pathFinding.CurrentStepIndex]);
                    movementTarget.IsActive = true;
                }
                else
                {
                    pathFinding.State = PathFindingState.AtDestination;
                }
            }
        }
    }
}
