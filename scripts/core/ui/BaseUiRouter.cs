using System.Collections.Generic;
using GFramework.Core.extensions;
using GFramework.Core.system;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// 抽象UI路由基类，提供页面栈管理功能
/// </summary>
/// <typeparam name="T">UI根节点类型，必须实现IUiRoot接口</typeparam>
public class BaseUiRouter<T> :AbstractSystem, IUiRouter where T : IUiRoot
{
    protected T UiRoot = default!;
    /// <summary>
    /// UI工厂实例，用于创建UI相关的对象
    /// </summary>
    protected IUiFactory Factory { get; private set; } = null!;
    
    /// <summary>
    /// 初始化方法，在页面初始化时获取UI工厂实例
    /// </summary>
    protected override void OnInit()
    {
        Factory = this.GetUtility<IUiFactory>()!;
    }

    /// <summary>
    /// 页面栈，用于管理UI页面的显示顺序
    /// </summary>
    private readonly Stack<IUiPage> _stack = new();

    /// <summary>
    /// 绑定根UI组件
    /// </summary>
    /// <param name="root">要绑定的根UI组件</param>
    public void BindRoot(T root)
    {
        UiRoot = root;
    }

    /// <summary>
    /// 将指定UI页面压入栈顶并显示
    /// </summary>
    /// <param name="uiKey">UI页面标识符</param>
    /// <param name="param">页面进入参数，可为空</param>
    public void Push(string uiKey, IUiPageEnterParam? param = null)
    {
        // 暂停当前栈顶页面
        if (_stack.Count > 0)
            _stack.Peek().OnPause();

        var page = Factory.Create(uiKey);
        UiRoot.AddUiPage(page);
        _stack.Push(page);

        page.OnEnter(param);
    }

    /// <summary>
    /// 弹出栈顶页面并销毁
    /// </summary>
    public void Pop()
    {
        if (_stack.Count == 0) return;

        var top = _stack.Pop();
        top.OnExit();
        UiRoot.RemoveUiPage(top);

        // 恢复新的栈顶页面
        if (_stack.Count > 0)
            _stack.Peek().OnResume();
    }

    /// <summary>
    /// 替换当前所有页面为新页面
    /// </summary>
    /// <param name="uiKey">新UI页面标识符</param>
    /// <param name="param">页面进入参数，可为空</param>
    public void Replace(string uiKey, IUiPageEnterParam? param = null)
    {
        Clear();
        Push(uiKey, param);
    }

    /// <summary>
    /// 清空所有页面栈中的页面
    /// </summary>
    public void Clear()
    {
        while (_stack.Count > 0)
            Pop();
    }
}
