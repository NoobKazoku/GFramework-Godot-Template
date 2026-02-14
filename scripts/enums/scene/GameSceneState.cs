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

namespace GFrameworkGodotTemplate.scripts.enums.scene;

/// <summary>
/// 场景状态枚举，用于表示游戏场景的不同运行状态。
/// 包含场景从加载到卸载的完整生命周期状态。
/// </summary>
public enum GameSceneState
{
    /// <summary>
    /// 场景正在加载中，资源尚未完全准备就绪
    /// </summary>
    Loading,

    /// <summary>
    /// 场景处于激活状态，正常运行和更新
    /// </summary>
    Active,

    /// <summary>
    /// 场景已暂停，停止所有逻辑更新但保持显示
    /// </summary>
    Paused,

    /// <summary>
    /// 场景被隐藏，不显示在屏幕上但仍保留在内存中
    /// </summary>
    Hidden,

    /// <summary>
    /// 场景正在卸载，清理资源并准备移除
    /// </summary>
    Unloading
}