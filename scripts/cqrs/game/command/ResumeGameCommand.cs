using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     恢复游戏命令类
/// </summary>
public sealed record ResumeGameCommand : ICommand;