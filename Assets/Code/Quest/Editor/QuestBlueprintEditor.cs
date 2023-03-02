using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(QuestBlueprint))]
public class QuestBlueprintEditor : Editor
{
    [SerializeField]
    private VisualTreeAsset m_EditorLayout;

    private List<string> m_ObjectivesChoices;
    private List<Type> m_ObjectivesTypes;
    private DropdownField m_ObjectiveTypesSelector;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement newInspector = new();
        m_EditorLayout.CloneTree(newInspector);

        QuestBlueprint questBlueprint = (QuestBlueprint)target;

        m_ObjectiveTypesSelector = newInspector.Q<DropdownField>();
        m_ObjectiveTypesSelector.choices = ComputeObjectiveTypes();
        m_ObjectiveTypesSelector.RegisterCallback<ChangeEvent<string>>(OnObjectiveTypeChanged);
        if (questBlueprint.RootObjective != null)
        {
            m_ObjectiveTypesSelector.value = questBlueprint.RootObjective.GetType().Name;
        }


        return newInspector;
    }

    private void OnObjectiveTypeChanged(ChangeEvent<string> e)
    {
        if (MathTools.InRange(m_ObjectiveTypesSelector.index, 0, m_ObjectivesTypes.Count - 1))
        {
            QuestBlueprint questBlueprint = (QuestBlueprint)target;
            Type newObjectiveType = m_ObjectivesTypes[m_ObjectiveTypesSelector.index];

            if (questBlueprint.RootObjective == null || newObjectiveType != questBlueprint.RootObjective.GetType())
            {
                if (questBlueprint.RootObjective != null)
                {
                    AssetDatabase.RemoveObjectFromAsset(questBlueprint.RootObjective);
                }

                questBlueprint.RootObjective = (QuestObjectiveBlueprint)CreateInstance(newObjectiveType);
                questBlueprint.RootObjective.name = newObjectiveType.Name;

                AssetDatabase.AddObjectToAsset(questBlueprint.RootObjective, questBlueprint);
                AssetDatabase.SaveAssets();
            }
        }
    }

    private List<string> ComputeObjectiveTypes()
    {
        if (m_ObjectivesChoices == null)
        {
            m_ObjectivesTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(QuestObjectiveBlueprint)))
                .ToList();

            m_ObjectivesChoices = new();
            foreach (var behaviourType in m_ObjectivesTypes)
            {
                m_ObjectivesChoices.Add(behaviourType.Name);
            }
        }

        return m_ObjectivesChoices;
    }
}
