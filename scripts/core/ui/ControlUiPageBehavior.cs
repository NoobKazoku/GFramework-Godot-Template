using GFramework.Godot.extensions;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// 控制UI页面行为的类，实现了IUiPage接口，用于管理Control节点的生命周期和状态
/// </summary>
/// <param name="owner">拥有该行为的Control节点</param>
public class ControlUiPageBehavior(Control owner) : IUiPage
{
    /// <summary>
    /// 当页面进入时调用的方法
    /// </summary>
    /// <param name="param">页面进入参数，可为空</param>
    public void OnEnter(IUiPageEnterParam? param) {}

    /// <summary>
    /// 当页面退出时调用的方法，将所有者节点加入释放队列
    /// </summary>
    public void OnExit()
    {
        owner.QueueFreeX();
    }

    /// <summary>
    /// 当页面暂停时调用的方法，禁用处理、物理处理和输入处理
    /// </summary>
    public void OnPause()
    {
        owner.SetProcess(enable: false);
        owner.SetPhysicsProcess(enable: false);
        owner.SetProcessInput(enable: false);
    }

    /// <summary>
    /// 当页面恢复时调用的方法，启用处理、物理处理和输入处理
    /// </summary>
    public void OnResume()
    {
        owner.SetProcess(enable: true);
        owner.SetPhysicsProcess(enable: true);
        owner.SetProcessInput(enable: true);
    }

    /// <summary>
    /// 当页面隐藏时调用的方法，隐藏所有者节点
    /// </summary>
    public void OnHide()
    {
        owner.Hide();
    }

    /// <summary>
    /// 当页面显示时调用的方法，显示所有者节点并恢复处理
    /// </summary>
    public void OnShow()
    {
        owner.Show();
        OnResume();
    }

    public bool IsAlive { get; } = owner.IsValidNode();
}
