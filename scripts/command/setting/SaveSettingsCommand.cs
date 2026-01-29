using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.setting;

namespace GFrameworkGodotTemplate.scripts.command.setting;

/// <summary>
/// 保存游戏设置命令类
/// 负责将当前游戏设置数据保存到存储中
/// </summary>
public sealed class SaveSettingsCommand : AbstractCommand
{
    /// <summary>
    /// 执行保存设置命令的主逻辑
    /// </summary>
    protected override void OnExecute()
    {
        _ = this.GetSystem<ISettingsSystem>()!.SaveAll();
    }
}
