using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.Quest.Editor
{
    public class QuestEditor : EditorWindow
    {
        private const string k_EditionPanelName = "ctr_EditionPanel";
        private const string k_RootObjectiveName = "ctr_RootObjective";
        private const string k_ToolboxListName = "lst_Toolbox";
        private const string k_NoSelectionMessageName = "lbl_NoSelectionMessage";
        private const string k_SelectedQuestContentName = "ctr_SelectedQuestContent";
        private const string k_MissingRootObjectiveName = "ctr_MissingRootObjective";
        private const string k_NewObjectiveName = "New Objective";

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField]
        private VisualTreeAsset m_ObjectiveVisualTreeAsset = default;

        private VisualElement m_EditionPanel;
        private VisualElement m_SelectedQuestContentElement;
        private VisualElement m_MissingRootObjectiveElement;
        private Label m_NoSelectionMessageLabel;
        private ListView m_ToolboxList;
        private QuestBlueprint m_SelectedQuest;
        private QuestObjectiveEditor m_RootObjective;
        private List<Type> m_ObjectivesTypes;
        private Dictionary<int, QuestEditorToolbarEntry> m_ToolbarEntries = new();

        [MenuItem("Fluffy Gamedev/Quest Editor")]
        public static void ShowExample()
        {
            QuestEditor wnd = GetWindow<QuestEditor>();
            wnd.titleContent = new GUIContent("QuestEditor");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            m_VisualTreeAsset.CloneTree(root);

            m_EditionPanel = root.Q(k_EditionPanelName);
            m_RootObjective = new(root.Q(k_RootObjectiveName), m_ObjectiveVisualTreeAsset);
            m_ToolboxList = root.Q<ListView>(k_ToolboxListName);
            m_SelectedQuestContentElement = root.Q(k_SelectedQuestContentName);
            m_MissingRootObjectiveElement = root.Q(k_MissingRootObjectiveName);
            m_NoSelectionMessageLabel = root.Q<Label>(k_NoSelectionMessageName);

            InitToolbox();

            if (Selection.activeObject is QuestBlueprint quest)
            {
                BindQuestToEditor(quest);
            }

            RefreshContentVisibility();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is QuestBlueprint quest)
            {
                UnbindQuestFromEditor();
                BindQuestToEditor(quest);

                RefreshContentVisibility();
            }
        }

        private void OnDestroy()
        {
            UnbindQuestFromEditor();
        }

        private void BindQuestToEditor(QuestBlueprint quest)
        {
            m_SelectedQuest = quest;

            if (EditorUtility.IsPersistent(m_SelectedQuest))
            {
                RefreshRootObjectiveVisibility();

                m_EditionPanel.Bind(new SerializedObject(quest));

                if (m_SelectedQuest.RootObjective != null)
                {
                    m_RootObjective.Bind(m_SelectedQuest.RootObjective);
                    m_RootObjective.OnSubObjectivesRemoved += OnRootObjectivesRemovedCallback;
                }
            }
            else
            {
                EditorApplication.update += OnDelayBindQuest;
            }
        }

        private void OnDelayBindQuest()
        {
            if (EditorUtility.IsPersistent(m_SelectedQuest))
            {
                EditorApplication.update -= OnDelayBindQuest;
                BindQuestToEditor(m_SelectedQuest);
            }
        }



        private void UnbindQuestFromEditor()
        {
            m_RootObjective.OnSubObjectivesRemoved -= OnRootObjectivesRemovedCallback;
            m_RootObjective.Unbind();
            m_EditionPanel.Unbind();
            m_SelectedQuest = null;
        }

        private void InitToolbox()
        {
            m_ObjectivesTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(QuestObjectiveBlueprint)))
                .ToList();

            m_ToolboxList.makeItem = MakeToolboxEntry;
            m_ToolboxList.bindItem = BindToolboxEntry;
            m_ToolboxList.unbindItem = UnbindToolboxEntry;

            m_ToolboxList.itemsSource = m_ObjectivesTypes;
        }

        private VisualElement MakeToolboxEntry()
        {
            return new Label();
        }

        private void BindToolboxEntry(VisualElement element, int index)
        {
            QuestEditorToolbarEntry newEntry = new();
            newEntry.Bind(m_EditionPanel, element, m_ObjectivesTypes[index]);
            newEntry.onDropObjective += OnDropObjective;

            m_ToolbarEntries.Add(index, newEntry);
        }

        private void UnbindToolboxEntry(VisualElement element, int index)
        {
            if (m_ToolbarEntries.Remove(index, out var removedEntry))
            {
                removedEntry.onDropObjective -= OnDropObjective;
                removedEntry.Unbind();
            }
        }

        private void OnDropObjective(Type objectiveType, VisualElement targetElement)
        {
            if (targetElement == m_MissingRootObjectiveElement)
            {
                QuestObjectiveBlueprint newObjective = (QuestObjectiveBlueprint)CreateInstance(objectiveType);
                newObjective.name = k_NewObjectiveName;
                m_SelectedQuest.RootObjective = newObjective;

                AssetDatabase.AddObjectToAsset(newObjective, m_SelectedQuest);
                EditorUtility.SetDirty(newObjective);
                EditorUtility.SetDirty(m_SelectedQuest);

                if (m_SelectedQuest.RootObjective != null)
                {
                    m_RootObjective.Bind(m_SelectedQuest.RootObjective);
                }

                RefreshRootObjectiveVisibility();
            }
            else
            {
                m_RootObjective.TryDropTool(objectiveType, targetElement);
            }
        }

        private void OnRootObjectivesRemovedCallback(QuestObjectiveEditor subObjective)
        {
            SerializedObject serializedObject = new(m_SelectedQuest);
            var rootobjectiveProperty = serializedObject.FindProperty("m_RootObjective");
            serializedObject.Update();

            rootobjectiveProperty.objectReferenceValue = null;

            serializedObject.ApplyModifiedProperties();

            RefreshRootObjectiveVisibility();
        }

        private void RefreshContentVisibility()
        {
            m_SelectedQuestContentElement.style.display = m_SelectedQuest != null ? DisplayStyle.Flex : DisplayStyle.None;
            m_NoSelectionMessageLabel.style.display = m_SelectedQuest != null ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void RefreshRootObjectiveVisibility()
        {
            m_RootObjective.ObjectiveElement.style.display = m_SelectedQuest.RootObjective != null ? DisplayStyle.Flex : DisplayStyle.None;
            m_MissingRootObjectiveElement.style.display = m_SelectedQuest.RootObjective != null ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
