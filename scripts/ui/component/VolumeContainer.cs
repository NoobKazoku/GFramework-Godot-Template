using Godot;

namespace GFrameworkGodotTemplate.scripts.ui.component;

/// <summary>
///     音量控制容器抽象类，提供音量滑块、标签和数值显示功能
/// </summary>
[ContextAware]
[Log]
public partial class VolumeContainer : HBoxContainer, IController
{
    /// <summary>
    ///     音量改变事件委托
    /// </summary>
    [Signal]
    public delegate void VolumeChangedEventHandler(float value);

    /// <summary>
    ///     获取音量标签控件
    /// </summary>
    [GetNode] private Label _volumeLabel = null!;

    /// <summary>
    ///     获取音量滑块控件
    /// </summary>
    [GetNode] private HSlider _volumeSlider = null!;

    /// <summary>
    ///     获取音量数值显示控件
    /// </summary>
    [GetNode] private Label _volumeValue = null!;

    /// <summary>
    ///     Godot节点就绪时调用的方法，初始化事件订阅
    /// </summary>
    public override void _Ready()
    {
        __InjectGetNodes_Generated();
        // 订阅滑块值改变事件
        _volumeSlider.ValueChanged += OnSliderValueChanged;
    }


    /// <summary>
    ///     初始化音量控制器
    /// </summary>
    /// <param name="title">音量控制器标题</param>
    /// <param name="initialValue">初始音量值</param>
    public void Initialize(string title, float initialValue)
    {
        _volumeLabel.Text = title;
        SetValue(initialValue);
    }

    /// <summary>
    ///     设置音量值
    /// </summary>
    /// <param name="value">要设置的音量值</param>
    public void SetValue(float value)
    {
        _volumeSlider.Value = value;
        UpdateValueText(value);
    }

    /// <summary>
    ///     滑块值改变时的回调方法
    /// </summary>
    /// <param name="value">新的滑块值</param>
    private void OnSliderValueChanged(double value)
    {
        var v = (float)value;
        UpdateValueText(v);
        EmitSignalVolumeChanged(v);
    }

    /// <summary>
    ///     更新音量数值显示文本
    /// </summary>
    /// <param name="value">当前音量值</param>
    private void UpdateValueText(float value)
    {
        _volumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
    }
}