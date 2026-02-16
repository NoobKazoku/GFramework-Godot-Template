using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command.input;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     暂停游戏并打开暂停菜单命令类
/// </summary>
/// <param name="input">打开暂停菜单命令输入参数</param>
public sealed class PauseGameWithOpenPauseMenuCommand(OpenPauseMenuCommandInput input)
    : CommandBase<OpenPauseMenuCommandInput, UiHandle>(input);