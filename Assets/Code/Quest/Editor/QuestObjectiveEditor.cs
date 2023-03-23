using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.Quest.Editor
{
    internal class QuestObjectiveEditor
    {
        private const string k_ObjectiveTypeName = "lbl_ObjectiveType";
        private const string k_SubObjectivesName = "ctr_SubObjectives";
        private const string k_DefaultFieldsName = "ctr_DefaultFields";
        private const string k_RemoveButtonName = "btn_RemoveButton";
        private const string k_HasSubObjectivesClass = "quest-objective__sub-objectives-list--has-sub-objectives";
        private const string k_NewObjectiveName = "New Objective";

        private Label m_ObjectiveTypeLabel;
        private VisualElement m_ObjectiveElement;
        private VisualElement m_SubObjectivesElement;
        private IMGUIContainer m_DefaultFieldsElement;
        private Button m_RemoveButton;
        private VisualTreeAsset m_ObjectiveVisualTreeAsset;

        private QuestObjectiveBlueprint m_BoundObjective;
        private List<QuestObjectiveEditor> m_SubObjectives = new();

        public VisualElement ObjectiveElement => m_ObjectiveElement;
        public event Action<QuestObjectiveEditor> OnSubObjectivesRemoved;

        public QuestObjectiveEditor(VisualElement objectiveElement, VisualTreeAsset objectiveVisualTreeAsset)
        {
            m_ObjectiveElement = objectiveElement;
            m_ObjectiveVisualTreeAsset = objectiveVisualTreeAsset;
            m_ObjectiveTypeLabel = m_ObjectiveElement.Q<Label>(k_ObjectiveTypeName);
            m_SubObjectivesElement = m_ObjectiveElement.Q(k_SubObjectivesName);
            m_DefaultFieldsElement = m_ObjectiveElement.Q<IMGUIContainer>(k_DefaultFieldsName);
            m_RemoveButton = m_ObjectiveElement.Q<Button>(k_RemoveButtonName);

            m_DefaultFieldsElement.onGUIHandler = OnDefaultGUI;
            m_RemoveButton.clicked += OnRemoveObjective;
        }

        public void Bind(QuestObjectiveBlueprint objective)
        {
            m_BoundObjective = objective;
            m_ObjectiveElement.Bind(new SerializedObject(m_BoundObjective));
            m_ObjectiveTypeLabel.text = m_BoundObjective.GetType().Name;

            if (m_BoundObjective.HasSubObjectives)
            {
                m_SubObjectivesElement.AddToClassList(k_HasSubObjectivesClass);
                if (m_BoundObjective.SubObjectives != null)
                {
                    foreach (var subObjective in m_BoundObjective.SubObjectives)
                    {
                        AddSubObjective(subObjective);
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
            m_SubObjectives.Clear();

            m_SubObjectivesElement.RemoveFromClassList(k_HasSubObjectivesClass);
            m_SubObjectivesElement.Clear();
            m_ObjectiveTypeLabel.text = string.Empty;
            m_ObjectiveElement.Unbind();
            m_BoundObjective = null;
        }

        public bool TryDropTool(Type toolType, VisualElement target)
        {
            bool hasFoundTarget = false;

            foreach (var subObjectiveEditor in m_SubObjectives)
            {
                if (subObjectiveEditor.TryDropTool(toolType, target))
                {
                    hasFoundTarget = true;
                    break;
                }
            }

            if (!hasFoundTarget && m_SubObjectivesElement == target)
            {
                hasFoundTarget = true;

                SerializedObject serializedObject = new(m_BoundObjective);
                var listProperty = serializedObject.FindProperty(m_BoundObjective.SubObjectivesPropertyName);
                serializedObject.Update();

                QuestObjectiveBlueprint newObjective = (QuestObjectiveBlueprint)ScriptableObject.CreateInstance(toolType);
                newObjective.name = k_NewObjectiveName;

                AddSubObjective(newObjective);

                AssetDatabase.AddObjectToAsset(newObjective, m_BoundObjective);

                ++listProperty.arraySize;
                var newProperty = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                newProperty.objectReferenceValue = newObjective;

                serializedObject.ApplyModifiedProperties();
            }

            return hasFoundTarget;
        }

        private void AddSubObjective(QuestObjectiveBlueprint subObjective)
        {
            VisualElement newObjective = m_ObjectiveVisualTreeAsset.Instantiate();
            QuestObjectiveEditor newObjectiveEditor = new QuestObjectiveEditor(newObjective[0], m_ObjectiveVisualTreeAsset);
            m_SubObjectivesElement.Add(newObjective);
            m_SubObjectives.Add(newObjectiveEditor);
            newObjectiveEditor.Bind(subObjective);
            newObjectiveEditor.OnSubObjectivesRemoved += OnSubObjectivesRemovedCallback;
        }

        private void OnDefaultGUI()
        {
            if (m_BoundObjective != null)
            {
                SerializedObject serializedObject = new(m_BoundObjective);
                serializedObject.Update();

                SerializedProperty currentProperty = serializedObject.GetIterator();
                currentProperty.NextVisible(true);
                while (currentProperty.NextVisible(false))
                {
                    EditorGUILayout.PropertyField(currentProperty);
                }


                serializedObject.ApplyModifiedProperties();
            }
        }

        private void RemoveChildren()
        {
            foreach (var subObjective in m_SubObjectives)
            {
                subObjective.RemoveChildren();
                AssetDatabase.RemoveObjectFromAsset(subObjective.m_BoundObjective);
                subObjective.Unbind();
            }
        }

        private void OnRemoveObjective()
        {
            RemoveChildren();
            OnSubObjectivesRemoved?.Invoke(this);
            AssetDatabase.RemoveObjectFromAsset(m_BoundObjective);
            Unbind();
        }

        private void OnSubObjectivesRemovedCallback(QuestObjectiveEditor subObjective)
        {
            if (m_BoundObjective != null)
            {
                SerializedObject serializedObject = new(m_BoundObjective);
                var listProperty = serializedObject.FindProperty(m_BoundObjective.SubObjectivesPropertyName);
                serializedObject.Update();

                int subObjectiveIndex = m_SubObjectives.IndexOf(subObjective);
                listProperty.DeleteArrayElementAtIndex(subObjectiveIndex);
                m_SubObjectives.RemoveAt(subObjectiveIndex);
                subObjective.m_ObjectiveElement.parent.Remove(subObjective.m_ObjectiveElement);

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
