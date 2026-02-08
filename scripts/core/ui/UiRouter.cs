using GFramework.Game.ui;
using GFramework.Game.ui.handler;
using GFramework.SourceGenerators.Abstractions.logging;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
///     UI路由类，提供页面栈管理功能
/// </summary>
[Log]
public partial class UiRouter : UiRouterBase
{
    /// <summary>
    ///     注册默认的过渡处理器
    /// </summary>
    /// <remarks>
    ///     该方法用于注册日志记录过渡处理器到UI路由系统中
    /// </remarks>
    protected override void RegisterHandlers()
    {
        // 注册默认过渡处理器时输出调试日志
        _log.Debug("Registering default transition handlers");
        RegisterHandler(new LoggingTransitionHandler());
    }
}