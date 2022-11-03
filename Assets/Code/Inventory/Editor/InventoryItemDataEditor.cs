using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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
        private ListView m_BehaviourList;
        private DropdownField m_BehaviourTypesSelector;
        private SerializedProperty m_ListProperty;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement newInspector = new();
            m_EditorLayout.CloneTree(newInspector);

            m_ListProperty = serializedObject.FindProperty("m_behaviourCreators");

            m_BehaviourList = newInspector.Q<ListView>();
            m_BehaviourList.makeItem = () => new VisualElement();
            m_BehaviourList.bindItem = BindBehaviour;
            m_BehaviourList.unbindItem = UnbindBehaviour;

            m_BehaviourTypesSelector = newInspector.Q<DropdownField>();
            m_BehaviourTypesSelector.choices = ComputeBehaviourTypes();

            Button addButton = newInspector.Q<Button>("bt_AddBehaviour");
            addButton.clicked += AddItemBehaviour;

            Button removeButton = newInspector.Q<Button>("bt_RemoveBehaviour");
            removeButton.clicked += RemoveItemBehaviour;

            return newInspector;
        }

        private void BindBehaviour(VisualElement element, int index)
        {
            ObjectField newField = new();
            newField.value = ((InventoryItemData)target).behaviourCreators[index];
            element.Add();
        }

        private void UnbindBehaviour(VisualElement element, int index)
        {
            element.Clear();
        }

        private List<string> ComputeBehaviourTypes()
        {
            if (m_BehaviourChoices == null)
            {
                m_BehaviourTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.GetInterfaces().Contains(typeof(InventoryItemBehaviourCreator)))
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
            //TODO: sanity checks!!!
            InventoryItemData itemData = (InventoryItemData)target;
            itemData.behaviourCreators.Add((InventoryItemBehaviourCreator)Activator.CreateInstance(m_BehaviourTypes[m_BehaviourTypesSelector.index]));
        }

        private void RemoveItemBehaviour()
        {
            //TODO: sanity checks!!!
            InventoryItemData itemData = (InventoryItemData)target;
            itemData.behaviourCreators.RemoveAt(m_BehaviourList.selectedIndex);
        }
    }
}
