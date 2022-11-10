
namespace FluffyGameDev.Escapists.Core
{
    public static class MathTools
    {
        public static bool InRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
