using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.Quest.Editor
{
    internal class QuestObjectiveEditor
    {
        private const string k_ObjectiveTypeName = "lbl_ObjectiveType";
        private const string k_ObjectiveNameElementName = "txt_ObjectiveName";
        private const string k_SubObjectivesName = "ctr_SubObjectives";
        private const string k_HasSubObjectivesClass = "quest-objective__sub-objectives-list--has-sub-objectives";

        private Label m_ObjectiveTypeLabel;
        private TextField m_ObjectiveNameTextField;
        private VisualElement m_ObjectiveElement;
        private VisualElement m_SubObjectivesElement;
        private VisualTreeAsset m_ObjectiveVisualTreeAsset;

        private QuestObjectiveBlueprint m_BoundObjective;
        private List<QuestObjectiveEditor> m_SubObjectives = new();

        public QuestObjectiveEditor(VisualElement objectiveElement, VisualTreeAsset objectiveVisualTreeAsset)
        {
            m_ObjectiveElement = objectiveElement;
            m_ObjectiveVisualTreeAsset = objectiveVisualTreeAsset;
            m_ObjectiveTypeLabel = m_ObjectiveElement.Q<Label>(k_ObjectiveTypeName);
            m_ObjectiveNameTextField = m_ObjectiveElement.Q<TextField>(k_ObjectiveNameElementName);
            m_SubObjectivesElement = m_ObjectiveElement.Q(k_SubObjectivesName);

            m_ObjectiveNameTextField.RegisterCallback<ChangeEvent<string>>(OnNameChange);
        }

        public void Bind(QuestObjectiveBlueprint objective)
        {
            m_BoundObjective = objective;
            m_ObjectiveElement.Bind(new SerializedObject(m_BoundObjective));
            m_ObjectiveTypeLabel.text = m_BoundObjective.GetType().Name;
            m_ObjectiveNameTextField.value = m_BoundObjective.name;

            if (m_BoundObjective.HasSubObjectives)
            {
                m_SubObjectivesElement.AddToClassList(k_HasSubObjectivesClass);
                if (m_BoundObjective.SubObjectives != null)
                {
                    foreach (var subObjective in m_BoundObjective.SubObjectives)
                    {
                        VisualElement newObjective = m_ObjectiveVisualTreeAsset.Instantiate();
                        QuestObjectiveEditor newObjectiveEditor = new QuestObjectiveEditor(newObjective[0], m_ObjectiveVisualTreeAsset);
                        m_SubObjectives.Add(newObjectiveEditor);
                        newObjectiveEditor.Bind(subObjective);
                    }
                }
            }
        }

        public void Unbind()
        {
            foreach (var subObjective in m_SubObjectives)
            {
                subObjective.Unbind();
            }

            m_SubObjectivesElement.RemoveFromClassList(k_HasSubObjectivesClass);
            m_SubObjectivesElement.Clear();
            m_ObjectiveNameTextField.value = string.Empty;
            m_ObjectiveTypeLabel.text = string.Empty;
            m_ObjectiveElement.Unbind();
            m_BoundObjective = null;
        }

        private void OnNameChange(ChangeEvent<string> evt)
        {
            if (m_BoundObjective != null)
            {
                m_BoundObjective.name = evt.newValue;
            }
        }
    }
}
