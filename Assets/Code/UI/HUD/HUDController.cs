using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument m_UIDocument;
        [SerializeField]
        private ActivitiesPresenter m_ActivitiesPresenter;
        [SerializeField]
        private InventoryPresenter m_InventoryPresenter;
        [SerializeField]
        private StatsPresenter m_StatsPresenter;
        [SerializeField]
        private ToolPresenter m_ToolPresenter;

        private VisualElement m_Root;
        private List<IHUDElementPresenter> m_HUDElements = new();

        private void Start()
        {
            m_Root = m_UIDocument.rootVisualElement;

            // TODO: make data driven
            RegisterHUDElement<ActivitiesPresenter, ActivitiesView>(m_ActivitiesPresenter, "ctr_activity");
            RegisterHUDElement<InventoryPresenter, InventoryView>(m_InventoryPresenter, "ctr_inventory");
            RegisterHUDElement<StatsPresenter, StatsView>(m_StatsPresenter, "ctr_Stats");
            RegisterHUDElement<ToolPresenter, ToolView>(m_ToolPresenter, "ctr_tool");
        }

        private void OnDestroy()
        {
            m_HUDElements.ForEach(element => element.Shutdown());
        }

        private void RegisterHUDElement<PresenterType, ViewType>(PresenterType presenter, string viewElementId)
            where PresenterType : HUDElementPresenter<ViewType>
            where ViewType : HUDElementView, new()
        {
            //TODO: Do we even need that ????
            ViewType view = new();
            view.root = m_Root.Q(viewElementId);
            presenter.view = view;
            presenter.Init();
            m_HUDElements.Add(presenter);
        }
    }
}
