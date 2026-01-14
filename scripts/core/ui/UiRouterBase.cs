using System.Collections.Generic;
using GFramework.Core.extensions;
using GFramework.Core.system;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// 抽象UI路由基类，提供页面栈管理功能
/// </summary>
/// <typeparam name="T">UI根节点类型，必须实现IUiRoot接口</typeparam>
public class UiRouterBase<T> :AbstractSystem, IUiRouter where T : IUiRoot
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
    /// <param name="policy">页面切换策略</param>
    public void Push(
        string uiKey,
        IUiPageEnterParam? param = null,
        UiTransitionPolicy policy = UiTransitionPolicy.Exclusive
    )
    {
        if (_stack.Count > 0)
        {
            var current = _stack.Peek();
            current.OnPause();

            if (policy == UiTransitionPolicy.Exclusive)
                current.OnHide();
        }

        var page = Factory.Create(uiKey);
        UiRoot.AddUiPage(page);
        _stack.Push(page);

        page.OnEnter(param);
        page.OnShow();
    }


        /// <summary>
    /// 弹出栈顶页面并根据策略处理页面
    /// </summary>
    /// <param name="policy">弹出策略，默认为销毁策略</param>
    public void Pop(UiPopPolicy policy = UiPopPolicy.Destroy)
    {
        if (_stack.Count == 0) return;

        var top = _stack.Pop();
        top.OnExit();

        // 根据弹出策略决定是销毁页面还是隐藏页面
        if (policy == UiPopPolicy.Destroy)
        {
            UiRoot.RemoveUiPage(top);
        }
        else
        {
            top.OnHide();
        }

        // 如果栈中还有页面，则将下一个页面设为当前活动页面
        if (_stack.Count > 0)
        {
            var next = _stack.Peek();
            next.OnResume();
            next.OnShow();
        }
    }



    /// <summary>
    /// 替换当前所有页面为新页面
    /// </summary>
    /// <param name="uiKey">新UI页面标识符</param>
    /// <param name="param">页面进入参数，可为空</param>
    /// <param name="popPolicy">弹出页面时的销毁策略，默认为销毁</param>
    /// <param name="pushPolicy">推入页面时的过渡策略，默认为独占</param>
    public void Replace(
        string uiKey,
        IUiPageEnterParam? param = null,
        UiPopPolicy popPolicy = UiPopPolicy.Destroy,
        UiTransitionPolicy pushPolicy = UiTransitionPolicy.Exclusive
    )
    {
        // 清空当前页面栈中的所有页面
        while (_stack.Count > 0)
            Pop(popPolicy);

        // 推入新的页面到栈中
        Push(uiKey, param, pushPolicy);
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
