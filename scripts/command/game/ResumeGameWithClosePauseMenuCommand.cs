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
using GFrameworkGodotTemplate.scripts.command.menu;
using GFrameworkGodotTemplate.scripts.command.menu.input;

namespace GFrameworkGodotTemplate.scripts.command.game;

/// <summary>
///     恢复游戏并关闭暂停菜单的命令类。
///     继承自 AbstractCommand，用于处理恢复游戏逻辑，并在执行时发送关闭暂停菜单和恢复游戏的命令。
/// </summary>
public class ResumeGameWithClosePauseMenuCommand(ClosePauseMenuCommandInput input)
    : AbstractCommand<ClosePauseMenuCommandInput>(input)
{
    /// <summary>
    ///     执行命令的核心逻辑。
    ///     首先发送关闭暂停菜单的命令，然后发送恢复游戏的命令。
    /// </summary>
    protected override void OnExecute(ClosePauseMenuCommandInput input)
    {
        // 发送关闭暂停菜单的命令
        this.SendCommand(new ClosePauseMenuCommand(input));

        // 发送恢复游戏的命令
        this.SendCommand(new ResumeGameCommand());
    }
}