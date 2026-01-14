using Godot;

namespace GFrameworkGodotTemplate.scripts.core.ui;

public partial class ControlUiPage : Godot.Control,IUiPage
{
    public override void _Ready()
    {
        Visible = false;
        SetProcess(enable: false);
        SetPhysicsProcess(enable: false);
        SetProcessInput(enable: false);
    }

    /// <summary>
    /// 页面进入
    /// </summary>
    public virtual void OnEnter(IUiPageEnterParam? param)
    {
        
    }

    /// <summary>
    /// 页面退出（默认销毁）
    /// </summary>
    public virtual void OnExit()
    {
        QueueFree();
    }

    /// <summary>
    /// 页面被覆盖
    /// </summary>
    public virtual void OnPause()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }

    /// <summary>
    /// 页面重新激活
    /// </summary>
    public virtual void OnResume()
    {
        SetProcess(enable: true);
        SetPhysicsProcess(enable: true);
        SetProcessInput(enable: true);
    }

    /// <summary>
    /// 页面隐藏（仍在栈中）
    /// </summary>
    public virtual void OnHide()
    {
        Hide();
    }

    /// <summary>
    /// 页面显示
    /// </summary>
    public virtual void OnShow()
    {
        Show();
        OnResume();
    }
}