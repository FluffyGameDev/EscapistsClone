using FluffyGameDev.Escapists.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.InventorySystem.Editor
{
    [CustomEditor(typeof(InventoryItemData))]
    public class InventoryItemDataEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset m_EditorLayout;

        private List<string> m_BehaviourChoices;
        private List<Type> m_BehaviourTypes;
        private IMGUIContainer m_ItemProperties;
        private ListView m_BehaviourList;
        private DropdownField m_BehaviourTypesSelector;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement newInspector = new();
            m_EditorLayout.CloneTree(newInspector);

            m_BehaviourList = newInspector.Q<ListView>();
            m_ItemProperties = newInspector.Q<IMGUIContainer>();
            m_ItemProperties.onGUIHandler = OnDrawItemPropertiesGUI;

            m_BehaviourTypesSelector = newInspector.Q<DropdownField>();
            m_BehaviourTypesSelector.choices = ComputeBehaviourTypes();

            Button addButton = newInspector.Q<Button>("bt_AddBehaviour");
            addButton.clicked += AddItemBehaviour;

            Button removeButton = newInspector.Q<Button>("bt_RemoveBehaviour");
            removeButton.clicked += RemoveItemBehaviour;

            return newInspector;
        }

        void OnDrawItemPropertiesGUI()
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty("m_ItemIcon");

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedProperty);
            serializedObject.ApplyModifiedProperties();
        }

        private List<string> ComputeBehaviourTypes()
        {
            if (m_BehaviourChoices == null)
            {
                m_BehaviourTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsSubclassOf(typeof(InventoryItemBehaviourCreator)))
                    .ToList();

                m_BehaviourChoices = new();
                foreach (var behaviourType in m_BehaviourTypes)
                {
                    m_BehaviourChoices.Add(behaviourType.Name);
                }
            }

            return m_BehaviourChoices;
        }

        private void AddItemBehaviour()
        {
            if (MathTools.InRange(m_BehaviourTypesSelector.index, 0, m_BehaviourTypes.Count - 1))
            {
                InventoryItemData itemData = (InventoryItemData)target;

                var usedType = m_BehaviourTypes[m_BehaviourTypesSelector.index];
                var createdBehavior = (InventoryItemBehaviourCreator)CreateInstance(usedType);
                createdBehavior.name = usedType.Name;
                itemData.behaviourCreators.Add(createdBehavior);

                AssetDatabase.AddObjectToAsset(createdBehavior, itemData);
                AssetDatabase.SaveAssets();
            }
        }

        private void RemoveItemBehaviour()
        {
            InventoryItemData itemData = (InventoryItemData)target;
            if (MathTools.InRange(m_BehaviourList.selectedIndex, 0, itemData.behaviourCreators.Count - 1))
            {
                AssetDatabase.RemoveObjectFromAsset(itemData.behaviourCreators[m_BehaviourList.selectedIndex]);
                itemData.behaviourCreators.RemoveAt(m_BehaviourList.selectedIndex);

                AssetDatabase.SaveAssets();
            }
        }
    }
}
