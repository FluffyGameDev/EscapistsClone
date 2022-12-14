using UnityEngine;

namespace FluffyGameDev.Escapists.Core
{
    public static class WorldUtils
    {
        private const float GridCellWidth = 0.32f;

        public static Vector3 SnapToGrid(Vector3 position)
        {
            Vector3 snapedPosition = position / GridCellWidth;
            snapedPosition.x = (Mathf.Floor(snapedPosition.x) + 0.5f) * GridCellWidth;
            snapedPosition.y = (Mathf.Floor(snapedPosition.y) + 0.5f) * GridCellWidth;
            return snapedPosition;
        }
    }
}
