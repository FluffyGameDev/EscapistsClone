using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace FluffyGameDev.Escapists.AI
{
    public class AgentAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed;

        class Baker : Baker<AgentAuthoring>
        {
            public override void Bake(AgentAuthoring authoring)
            {
                AddComponent(new AgentComponent());
                AddComponent(new MovementTargetComponent() { Speed = authoring.m_Speed });
                SetComponentEnabled<MovementTargetComponent>(false);
            }
        }
    }
}
