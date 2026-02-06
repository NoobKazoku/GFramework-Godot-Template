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
using GFrameworkGodotTemplate.scripts.pause_menu;

namespace GFrameworkGodotTemplate.scripts.command.menu;

/// <summary>
/// 打开暂停菜单的命令类。
/// 继承自 AbstractCommand，用于执行打开暂停菜单的操作。
/// </summary>
public class OpenPauseMenuCommand : AbstractCommand
{
    /// <summary>
    /// 执行打开暂停菜单的核心逻辑。
    /// 通过获取 UI 路由系统实例，显示指定的暂停菜单界面。
    /// </summary>
    protected override void OnExecute()
    {
        // 获取 UI 路由系统实例，并调用 Show 方法显示暂停菜单
        // 参数说明：
        // - PauseMenu.UiKeyStr：暂停菜单的唯一标识符
        // - UiLayer.Modal：指定菜单显示在模态层
        // - param: null：传递给菜单的参数为空
        // - instancePolicy: UiInstancePolicy.Reuse：复用已存在的菜单实例
        var uiRouter = this.GetSystem<IUiRouter>()!;
        var isExist = uiRouter.GetFromLayer(PauseMenu.UiKeyStr, UiLayer.Modal) is not null;
        if (isExist)
        {
            uiRouter.Resume(PauseMenu.UiKeyStr, UiLayer.Modal);
        }
        else
        {
            uiRouter.Show(PauseMenu.UiKeyStr, UiLayer.Modal);
        }
    }
}