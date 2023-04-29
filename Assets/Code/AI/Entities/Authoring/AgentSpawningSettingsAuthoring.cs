using UnityEngine;
using Unity.Entities;
using FluffyGameDev.Escapists.World;
using System.Collections.Generic;
using Unity.Collections;

namespace FluffyGameDev.Escapists.AI
{
    public class AgentSpawningSettingsAuthoring : MonoBehaviour
    {
        [SerializeField]
        private ActivityIdentifier m_CellActivity;
        [SerializeField]
        private AgentRoleIdentifier m_InmateRoleIdentifier;
        [SerializeField]
        private AgentRoleIdentifier m_GuardRoleIdentifier;
        [SerializeField]
        private List<AgentJobIdentifier> m_AvailableJobIdentifiers;
        [SerializeField]
        private float m_AgentSpeed;

        class Baker : Baker<AgentSpawningSettingsAuthoring>
        {
            public override void Bake(AgentSpawningSettingsAuthoring authoring)
            {
                NativeArray<int> jobIds = new NativeArray<int>(authoring.m_AvailableJobIdentifiers.Count, Allocator.Persistent);
                for (int i = 0; i < jobIds.Length; ++i)
                {
                    jobIds[i] = authoring.m_AvailableJobIdentifiers[i].JobId;
                }

                AddComponent(new AgentSpawningSettingsComponent()
                {
                    AgentSpeed = authoring.m_AgentSpeed,
                    InmateRoleId = authoring.m_InmateRoleIdentifier.RoleId,
                    GuardRoleId = authoring.m_GuardRoleIdentifier.RoleId,
                    CellActivityId = authoring.m_CellActivity.ActivityId,
                    AvailableJobIds = jobIds
                });
            }
        }
    }
}
