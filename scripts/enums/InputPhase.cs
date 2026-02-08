// Copyright (c) 2025 GeWuYou
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

namespace GFrameworkGodotTemplate.scripts.enums;

/// <summary>
///     定义输入处理的不同阶段，用于区分游戏逻辑中输入事件的优先级和状态。
/// </summary>
public enum InputPhase
{
    /// <summary>
    ///     全局输入处理阶段，始终最先执行，不受游戏状态影响。
    /// </summary>
    Global,

    /// <summary>
    ///     游戏进行中的输入处理阶段，仅在游戏未暂停时生效。
    /// </summary>
    Gameplay,

    /// <summary>
    ///     暂停状态下的输入处理阶段，仅在游戏暂停时生效。
    /// </summary>
    Paused
}