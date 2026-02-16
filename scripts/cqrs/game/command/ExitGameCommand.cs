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

using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.game.command;

/// <summary>
///     退出游戏命令记录类，用于表示退出游戏的指令
///     继承自ICommand接口，作为CQRS模式中的命令对象
/// </summary>
public sealed record ExitGameCommand : ICommand;