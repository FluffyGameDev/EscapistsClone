using FluffyGameDev.Escapists.Stats;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class PlayerBlackboardUpdater : MonoBehaviour
    {
        [SerializeField]
        private StatDescriptor m_HealthStatDescriptor;

        private PlayerStateMachineHolder m_PlayerStateMachineHolder;
        private Animator m_Animator;
        private Stat m_HealthStat;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_PlayerStateMachineHolder = GetComponent<PlayerStateMachineHolder>();

            StatHolder statHolder = GetComponent<StatHolder>();
            m_HealthStat = statHolder.Stats.GetStat(m_HealthStatDescriptor);
            m_HealthStat.OnStatChanged += OnUpdateHealth;

            m_PlayerStateMachineHolder.blackboard.Set((int)PlayerBB.Health, m_HealthStat.GetValueInt());

            m_PlayerStateMachineHolder.blackboard.RegisterEntryChangedCallback((int)PlayerBB.IsUsingTool, OnBBSet_IsUsingTool);
        }

        private void OnDestroy()
        {
            m_PlayerStateMachineHolder.blackboard.UnregisterEntryChangedCallback((int)PlayerBB.IsUsingTool, OnBBSet_IsUsingTool);

            m_HealthStat.OnStatChanged -= OnUpdateHealth;
            m_HealthStat = null;
        }

        private void OnUpdateHealth(Stat healthStat)
        {
            m_PlayerStateMachineHolder.blackboard.Set((int)PlayerBB.Health, healthStat.GetValueInt());
            m_Animator.SetInteger("Health", healthStat.GetValueInt());
        }

        private void OnBBSet_IsUsingTool(int bbIndex)
        {
            m_Animator.SetBool("UsingTool", m_PlayerStateMachineHolder.blackboard.Get<bool>(bbIndex));
        }
    }
}
