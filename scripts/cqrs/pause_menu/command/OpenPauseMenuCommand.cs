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

using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command.input;
using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command;

/// <summary>
///     打开暂停菜单命令类，用于表示打开暂停菜单的指令
///     继承自ICommand接口，作为CQRS模式中的命令对象，返回UiHandle类型的结果
/// </summary>
/// <param name="input">打开暂停菜单命令的输入参数</param>
public sealed class OpenPauseMenuCommand(OpenPauseMenuCommandInput input) : ICommand<UiHandle>
{
    /// <summary>
    ///     获取打开暂停菜单命令的输入参数
    /// </summary>
    public OpenPauseMenuCommandInput Input => input;
}