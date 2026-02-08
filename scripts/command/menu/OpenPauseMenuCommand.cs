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

using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.command.menu.input;
using GFrameworkGodotTemplate.scripts.pause_menu;

namespace GFrameworkGodotTemplate.scripts.command.menu;

/// <summary>
///     打开暂停菜单的命令类。
///     继承自 AbstractCommand，用于执行打开暂停菜单的操作。
/// </summary>
public class OpenPauseMenuCommand(OpenPauseMenuCommandInput input)
    : AbstractCommand<OpenPauseMenuCommandInput, UiHandle>(input)
{
    /// <summary>
    ///     执行打开暂停菜单的核心逻辑。
    ///     通过获取 UI 路由系统实例，显示指定的暂停菜单界面。
    /// </summary>
    /// <param name="input">包含执行所需数据的输入对象，其中可能包含一个可选的 UI 句柄。</param>
    /// <returns>返回一个 UiHandle 对象，表示当前显示或恢复的暂停菜单界面句柄。</returns>
    protected override UiHandle OnExecute(OpenPauseMenuCommandInput input)
    {
        // 获取 UI 路由系统实例，用于管理界面的显示与隐藏。
        var uiRouter = this.GetSystem<IUiRouter>()!;
        var handle = input.Handle;

        // 如果输入中未提供句柄，则直接显示暂停菜单界面。
        if (!handle.HasValue) return uiRouter.Show(PauseMenu.UiKeyStr, UiLayer.Modal);

        // 获取输入中的句柄值。
        var h = handle.Value;

        // 检查指定句柄是否已在模态层中存在，若不存在则显示暂停菜单界面。
        if (uiRouter.GetFromLayer(h, UiLayer.Modal) is null) return uiRouter.Show(PauseMenu.UiKeyStr, UiLayer.Modal);

        // 若句柄已存在于模态层中，则恢复该界面并返回句柄。
        uiRouter.Resume(h, UiLayer.Modal);
        return h;
    }
}