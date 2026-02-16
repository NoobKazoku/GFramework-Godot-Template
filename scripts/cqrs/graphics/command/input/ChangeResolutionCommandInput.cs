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

using GFramework.Core.Abstractions.cqrs.command;

namespace GFrameworkGodotTemplate.scripts.cqrs.graphics.command.input;

/// <summary>
///     分辨率更改命令输入类，用于传递分辨率更改所需的参数
/// </summary>
public sealed class ChangeResolutionCommandInput : ICommandInput
{
    /// <summary>
    ///     获取或设置分辨率的宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    ///     获取或设置分辨率的高度
    /// </summary>
    public int Height { get; set; }
}