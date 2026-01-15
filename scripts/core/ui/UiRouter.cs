using GFramework.Game.ui;
using GFramework.Game.ui.handler;
using GFramework.SourceGenerators.Abstractions.logging;

namespace GFrameworkGodotTemplate.scripts.core.ui;

/// <summary>
/// UI路由类，提供页面栈管理功能
/// </summary>
[Log]
public partial class UiRouter : UiRouterBase
{
    protected override void RegisterHandlers()
    {
        _log.Debug("Registering default transition handlers");
        RegisterHandler(new LoggingTransitionHandler());
    }
}