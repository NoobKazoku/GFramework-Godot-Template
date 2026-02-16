using GFramework.Core.cqrs.command;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     恢复游戏并关闭暂停菜单命令类
/// </summary>
/// <param name="input">关闭暂停菜单命令输入参数</param>
public sealed class ResumeGameWithClosePauseMenuCommand(ClosePauseMenuCommandInput input)
    : CommandBase<ClosePauseMenuCommandInput, Unit>(input);