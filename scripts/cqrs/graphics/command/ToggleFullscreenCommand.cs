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
using GFrameworkGodotTemplate.scripts.cqrs.graphics.command.input;
using Unit = Mediator.Unit;

namespace GFrameworkGodotTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     切换全屏模式命令类
///     继承自CommandBase，用于处理切换游戏窗口全屏状态的命令
/// </summary>
/// <param name="input">切换全屏命令的输入参数，包含执行命令所需的数据</param>
public class ToggleFullscreenCommand(ToggleFullscreenCommandInput input)
    : CommandBase<ToggleFullscreenCommandInput, Unit>(input);