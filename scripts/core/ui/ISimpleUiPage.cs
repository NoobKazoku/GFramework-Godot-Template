using GFramework.Game.Abstractions.ui;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
///     简单UI页面接口，继承自IUiPage接口
///     提供了页面生命周期管理的基础方法实现
/// </summary>
public interface ISimpleUiPage : IUiPage
{
    /// <summary>
    ///     页面退出时调用的方法
    /// </summary>
    void IUiPage.OnExit()
    {
    }

    /// <summary>
    ///     页面暂停时调用的方法
    /// </summary>
    void IUiPage.OnPause()
    {
    }

    /// <summary>
    ///     页面恢复时调用的方法
    /// </summary>
    void IUiPage.OnResume()
    {
    }

    /// <summary>
    ///     页面显示时调用的方法
    /// </summary>
    void IUiPage.OnShow()
    {
    }

    /// <summary>
    ///     页面隐藏时调用的方法
    /// </summary>
    void IUiPage.OnHide()
    {
    }

    /// <summary>
    ///     页面进入时调用的方法
    /// </summary>
    /// <param name="param">页面进入参数，可为空</param>
    void IUiPage.OnEnter(IUiPageEnterParam? param)
    {
    }
}