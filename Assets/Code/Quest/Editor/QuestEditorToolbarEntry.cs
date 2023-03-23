using System;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestEditorToolbarEntry
{
    private const string k_DropZoneUssClassName = "quest-objective__drop-zone";

    private VisualElement m_Root;
    private VisualElement m_EntryElement;
    private Type m_ToolType;

    public event Action<Type, VisualElement> onDropObjective;

    public void Bind(VisualElement root, VisualElement entryElement, Type toolType)
    {
        m_Root = root;
        m_EntryElement = entryElement;
        m_ToolType = toolType;

        Label label = m_EntryElement.Q<Label>();
        label.text = toolType.Name;
        m_EntryElement.RegisterCallback<PointerDownEvent>(OnPointerDown);
        m_EntryElement.RegisterCallback<PointerUpEvent>(OnPointerUp);
        m_EntryElement.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
    }

    public void Unbind()
    {
        m_EntryElement.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        m_EntryElement.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        m_EntryElement.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        evt.target.CapturePointer(evt.pointerId);
    }

    Vector2 m_DropLocalPosition;
    void OnPointerUp(PointerUpEvent evt)
    {
        if (evt.target.HasPointerCapture(evt.pointerId))
        {
            evt.target.ReleasePointer(evt.pointerId);
            m_DropLocalPosition = evt.localPosition;
        }
    }

    void OnPointerCaptureOut(PointerCaptureOutEvent evt)
    {
        UQueryBuilder<VisualElement> allDropZones = m_Root.Query(className: k_DropZoneUssClassName);
        UQueryBuilder<VisualElement> overlappingDropZones = allDropZones.Where(MouseInTarget);
        VisualElement targetZone = overlappingDropZones.Last();

        if (targetZone != null)
        {
            onDropObjective?.Invoke(m_ToolType, targetZone);
        }
    }

    private bool MouseInTarget(VisualElement element)
    {
        Vector2 mouseLocalPosition = m_EntryElement.ChangeCoordinatesTo(element, m_DropLocalPosition);
        return element.ContainsPoint(mouseLocalPosition);
    }
}
