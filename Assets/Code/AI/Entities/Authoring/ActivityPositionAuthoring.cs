using UnityEngine;
using Unity.Entities;
using FluffyGameDev.Escapists.World;

namespace FluffyGameDev.Escapists.AI
{
    public class ActivityPositionAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ActivityIdentifier m_ActivityIdentifier;
        [SerializeField]
        private AgentJobIdentifier m_JobIdentifier;
        [SerializeField]
        private AgentRoleIdentifier m_RoleIdentifier;
        [SerializeField]
        private int m_AgentId = AgentComponent.InvalidAgentId;
        [SerializeField]
        private float m_IdleMinDuration;
        [SerializeField]
        private float m_IdleMaxDuration;
        [SerializeField]
        private bool m_CanBeReserved;

        class Baker : Baker<ActivityPositionAuthoring>
        {
            public override void Bake(ActivityPositionAuthoring authoring)
            {
                if (authoring.m_ActivityIdentifier != null)
                {
                    AddComponent(new ActivityPositionComponent()
                    {
                        ActivityId = authoring.m_ActivityIdentifier.ActivityId,
                        AgentJobId = authoring.m_JobIdentifier != null ? authoring.m_JobIdentifier.JobId : AgentJobIdentifier.InvalidJobId,
                        AgentRoleId = authoring.m_RoleIdentifier != null ? authoring.m_RoleIdentifier.RoleId : AgentRoleIdentifier.InvalidRoleId,
                        IdleMinDuration = authoring.m_IdleMinDuration,
                        IdleMaxDuration = authoring.m_IdleMaxDuration,
                        OwnerAgentId = authoring.m_AgentId,
                        CanBeReserved = authoring.m_CanBeReserved
                    });
                }
            }
        }
    }
}
