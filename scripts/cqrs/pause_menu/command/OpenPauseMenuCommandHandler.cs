// Copyright (c) 2026 GeWuYou
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.pause_menu;

namespace GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command;

/// <summary>
///     打开暂停菜单命令处理器类，负责处理打开暂停菜单的命令逻辑
///     继承自AbstractCommandHandler，专门处理OpenPauseMenuCommand类型的命令，返回UiHandle结果
/// </summary>
public sealed class OpenPauseMenuCommandHandler
    : AbstractCommandHandler<OpenPauseMenuCommand, UiHandle>
{
    private IUiRouter? _uiRouter;

    /// <summary>
    ///     处理打开暂停菜单命令的核心方法
    ///     根据输入参数判断是否需要显示新的暂停菜单或恢复已存在的菜单
    /// </summary>
    /// <param name="command">打开暂停菜单命令对象，包含处理所需的输入参数</param>
    /// <param name="cancellationToken">取消令牌，用于取消操作</param>
    /// <returns>表示异步操作完成的ValueTask，返回UiHandle值表示UI句柄</returns>
    public override ValueTask<UiHandle> Handle(
        OpenPauseMenuCommand command,
        CancellationToken cancellationToken)
    {
        var input = command.Input;
        var handle = input.Handle;
        _uiRouter ??= this.GetSystem<IUiRouter>();
        if (!handle.HasValue)
            return ValueTask.FromResult(
                _uiRouter!.Show(PauseMenu.UiKeyStr, UiLayer.Modal));

        var h = handle.Value;

        if (_uiRouter!.GetFromLayer(h, UiLayer.Modal) is null)
            return ValueTask.FromResult(
                _uiRouter.Show(PauseMenu.UiKeyStr, UiLayer.Modal));

        _uiRouter.Resume(h, UiLayer.Modal);

        return ValueTask.FromResult(h);
    }
}