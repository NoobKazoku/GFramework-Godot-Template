namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI页面接口，定义了UI页面的生命周期方法
/// </summary>
public interface IUiPage
{
    /// <summary>
    /// 页面进入时调用的方法
    /// </summary>
    /// <param name="param">页面进入时传递的参数，可为空</param>
    public void OnEnter(IUiPageEnterParam? param);
    
    /// <summary>
    /// 页面退出时调用的方法
    /// </summary>
    public void OnExit() {}
    
    /// <summary>
    /// 页面暂停时调用的方法
    /// </summary>
    public void OnPause() {}
    
    /// <summary>
    /// 页面恢复时调用的方法
    /// </summary>
    public void OnResume() {}
}
