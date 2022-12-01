using FluffyGameDev.Escapists.InventorySystem;
using UnityEngine;

namespace FluffyGameDev.Escapists
{
    public class DurabilityItemBehaviourCreator : InventoryItemBehaviourCreator
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float m_DamagePerUse;

        public override InventoryItemBehaviour Create(InventoryItem owner)
        {
            return new DurabilityItemBehaviour(owner, m_DamagePerUse);
        }
    }

    public class DurabilityItemBehaviour : InventoryItemBehaviour
    {
        float m_DamagePerUse;

        private float m_Damage;
        public float damage => m_Damage;
        public bool isBroken => m_Damage >= 1.0f;

        public DurabilityItemBehaviour(InventoryItem owner, float damagePerUse)
            : base(owner)
        {
            m_DamagePerUse = damagePerUse;
        }

        public void ApplyUseDamage()
        {
            m_Damage += m_DamagePerUse;
        }
    }
}
