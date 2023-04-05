using UnityEngine;
using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public class AgentAuthoring : MonoBehaviour
    {
        class Baker : Baker<AgentAuthoring>
        {
            public override void Bake(AgentAuthoring authoring)
            {
                AddComponent(new AgentComponent());
            }
        }
    }
}
