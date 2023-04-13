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
        private float m_IdleMinDuration;
        [SerializeField]
        private float m_IdleMaxDuration;

        class Baker : Baker<ActivityPositionAuthoring>
        {
            public override void Bake(ActivityPositionAuthoring authoring)
            {
                if (authoring.m_ActivityIdentifier != null)
                {
                    AddComponent(new ActivityPositionComponent()
                    {
                        ActivityId = authoring.m_ActivityIdentifier.ActivityId,
                        IdleMinDuration = authoring.m_IdleMinDuration,
                        IdleMaxDuration = authoring.m_IdleMaxDuration
                    });
                }
            }
        }
    }
}
