using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public struct WorldActivityStepBufferElement : IBufferElementData
    {
        public int ActivityId;
        //TODO: job Id, role Id (Guard), property Id (ex: beds)
    }
}
