using Godot;

namespace GFrameworkGodotTemplate.scripts.ui.component;

/// <summary>
///     顶部栏组件，用于管理左右两侧的控件布局。
///     继承自 Godot 的 HBoxContainer，提供便捷的方法向左右容器添加子节点。
/// </summary>
public partial class TopBar : HBoxContainer
{
    /// <summary>
    ///     中间容器节点引用。
    /// </summary>
    [GetNode] private HBoxContainer _centerContainer = null!;

    /// <summary>
    ///     左侧容器节点引用。
    /// </summary>
    [GetNode] private HBoxContainer _leftContainer = null!;

    /// <summary>
    ///     右侧容器节点引用。
    /// </summary>
    [GetNode] private HBoxContainer _rightContainer = null!;

    /// <summary>
    ///     当节点进入场景树时调用，初始化左右容器节点的引用。
    /// </summary>
    public override void _Ready()
    {
        __InjectGetNodes_Generated();
    }

    /// <summary>
    ///     向左侧容器添加一个控制节点。
    /// </summary>
    /// <param name="node">要添加的控制节点。</param>
    public void AddLeft(Control node)
    {
        _leftContainer.AddChild(node);
    }

    /// <summary>
    ///     向中间容器添加一个控制节点。
    /// </summary>
    /// <param name="node">要添加的控制节点。</param>
    public void AddCenter(Control node)
    {
        _centerContainer.AddChild(node);
    }

    /// <summary>
    ///     向右侧容器添加一个控制节点。
    /// </summary>
    /// <param name="node">要添加的控制节点。</param>
    public void AddRight(Control node)
    {
        _rightContainer.AddChild(node);
    }
}