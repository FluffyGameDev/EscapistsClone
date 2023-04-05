using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.AI
{
    public class AgentBridgeBehaviourPool : MonoBehaviour
    {
        private static AgentBridgeBehaviourPool s_Instance;
        public static AgentBridgeBehaviourPool Instance => s_Instance;

        [SerializeField]
        private AgentBridgeBehaviour m_InstancePrefab;

        private List<AgentBridgeBehaviour> m_AllInstances = new();
        private Stack<int> m_FreeInstanceIds = new();

        void Awake()
        {
            //TODO: turn into service
            s_Instance = this;
        }

        public int AcquireInstance()
        {
            int instanceId;
            if (m_FreeInstanceIds.Count == 0)
            {
                instanceId = m_AllInstances.Count;
                m_AllInstances.Add(Instantiate(m_InstancePrefab, transform));
            }
            else
            {
                instanceId = m_FreeInstanceIds.Pop();
            }
            return instanceId;
        }

        public void FreeInstance(int instanceId)
        {
            if (instanceId < m_AllInstances.Count)
            {
                m_FreeInstanceIds.Push(instanceId);
            }
            else
            {
                //TODO error
            }
        }

        public AgentBridgeBehaviour GetInstance(int instanceId)
        {
            AgentBridgeBehaviour agentBridgeBehaviour = null;
            if (instanceId < m_AllInstances.Count)
            {
                agentBridgeBehaviour = m_AllInstances[instanceId];
            }
            else
            {
                //TODO error
            }
            return agentBridgeBehaviour;
        }
    }
}
