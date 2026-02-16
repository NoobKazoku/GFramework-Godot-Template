using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.command;

/// <summary>
///     重置所有设置命令类
/// </summary>
public sealed record ResetAllSettingsCommand : ICommand;