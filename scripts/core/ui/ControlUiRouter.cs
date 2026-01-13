namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// 控制UI路由器类，用于管理UI控件的路由和导航
/// </summary>
/// <param name="uiRoot">UI根节点，作为UI层次结构的根容器</param>
public class ControlUiRouter(scripts.ui.ControlUiRoot uiRoot)
    : AbstractUiRouter<scripts.ui.ControlUiRoot>(uiRoot);
