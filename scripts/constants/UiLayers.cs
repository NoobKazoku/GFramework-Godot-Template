namespace GFrameworkGodotTemplate.scripts.constants;

/// <summary>
///     定义UI层级常量的静态类，用于管理不同UI元素的渲染层级
/// </summary>
public static class UiLayers
{
    /// <summary>
    ///     世界层级，用于场景中的世界空间UI元素
    /// </summary>
    public const int World = 0;

    /// <summary>
    ///     UI根层级，用于基础UI界面元素
    /// </summary>
    public const int UiRoot = 100;

    /// <summary>
    ///     调试层级，用于调试信息显示等开发用UI元素
    /// </summary>
    public const int Debug = 200;
}