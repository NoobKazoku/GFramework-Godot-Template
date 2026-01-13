namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI路由管理器接口，用于管理UI界面的导航和切换操作
/// </summary>
public interface IUiRouter
{
    /// <summary>
    /// 将指定的UI界面压入路由栈，显示新的UI界面
    /// </summary>
    /// <param name="uiKey">UI界面的唯一标识符</param>
    /// <param name="param">进入界面的参数，可为空</param>
    void Push(string uiKey,IUiPageEnterParam? param=null);
    
    /// <summary>
    /// 弹出路由栈顶的UI界面，返回到上一个界面
    /// </summary>
    void Pop();
    
    /// <summary>
    /// 替换当前UI界面为指定的新界面
    /// </summary>
    /// <param name="uiKey">新UI界面的唯一标识符</param>
    /// <param name="param">进入界面的参数，可为空</param>
    void Replace(string uiKey,IUiPageEnterParam? param=null);
    
    /// <summary>
    /// 清空所有UI界面，重置路由状态
    /// </summary>
    void Clear();
}
