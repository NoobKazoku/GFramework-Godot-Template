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

namespace GFrameworkGodotTemplate.scripts.command.menu;

/// <summary>
///     关闭暂停菜单的命令类。
///     该类继承自 AbstractCommand，用于执行关闭暂停菜单的操作。
/// </summary>
public class ClosePauseMenuCommand(ClosePauseMenuCommandInput input)
    : AbstractCommand<ClosePauseMenuCommandInput>(input)
{
    /// <summary>
    ///     执行关闭暂停菜单的具体逻辑。
    ///     通过获取 UI 路由系统，隐藏指定的暂停菜单界面。
    /// </summary>
    protected override void OnExecute(ClosePauseMenuCommandInput input)
    {
        // 获取 UI 路由系统实例，并调用 Hide 方法隐藏暂停菜单界面
        this.GetSystem<IUiRouter>()!
            .Hide(input.Handle, UiLayer.Modal);
    }
}