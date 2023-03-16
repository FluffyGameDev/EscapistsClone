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
        private const string k_NewObjectiveText = "New Objective";

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField]
        private VisualTreeAsset m_ObjectiveVisualTreeAsset = default;

        private VisualElement m_EditionPanel;
        private ListView m_ToolboxList;
        private QuestBlueprint m_SelectedQuest;
        private QuestObjectiveEditor m_RootObjective;
        private List<Type> m_ObjectivesTypes;

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

            InitToolbox();

            if (Selection.activeObject is QuestBlueprint quest)
            {
                BindQuestToEditor(quest);
            }
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is QuestBlueprint quest)
            {
                UnbindQuestFromEditor();
                BindQuestToEditor(quest);
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
                if (m_SelectedQuest.RootObjective == null)
                {
                    m_SelectedQuest.RootObjective = CreateInstance<GroupQuestObjectiveBlueprint>();
                    m_SelectedQuest.RootObjective.name = k_NewObjectiveText;
                    AssetDatabase.AddObjectToAsset(m_SelectedQuest.RootObjective, m_SelectedQuest);
                    EditorUtility.SetDirty(m_SelectedQuest);
                    AssetDatabase.SaveAssetIfDirty(m_SelectedQuest);
                }

                m_EditionPanel.Bind(new SerializedObject(quest));
                m_RootObjective.Bind(m_SelectedQuest.RootObjective);
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

            m_ToolboxList.itemsSource = m_ObjectivesTypes;
        }
    }
}
