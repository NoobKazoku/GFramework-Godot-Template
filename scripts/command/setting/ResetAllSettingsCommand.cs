using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.setting;

namespace GFrameworkGodotTemplate.scripts.command.setting;

/// <summary>
///     重置设置命令类
/// </summary>
/// <param name="input">重置设置命令的输入参数</param>
public sealed class ResetAllSettingsCommand(EmptyCommandInput input)
    : AbstractAsyncCommand<EmptyCommandInput>(input)
{
    /// <summary>
    ///     执行重置设置命令的逻辑
    /// </summary>
    /// <param name="input">重置设置命令的输入参数</param>
    /// <returns>无返回值</returns>
    protected override async Task OnExecuteAsync(EmptyCommandInput input)
    {
        await this.GetSystem<ISettingsSystem>()!.ResetAll().ConfigureAwait(false);
    }
}