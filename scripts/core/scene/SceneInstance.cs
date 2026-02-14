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

using GFrameworkGodotTemplate.scripts.core.resource;
using GFrameworkGodotTemplate.scripts.enums.scene;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.scene;

/// <summary>
/// 场景实例类，用于管理单个场景的运行时信息和状态。
/// 包含场景的关键信息、节点引用、配置数据以及生命周期状态。
/// </summary>
public class SceneInstance
{
    /// <summary>
    /// 场景的唯一标识键
    /// </summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// 场景对应的Godot节点实例
    /// </summary>
    public Node? Node { get; init; }

    /// <summary>
    /// 场景配置信息
    /// </summary>
    public SceneConfig Config { get; init; } = new();

    /// <summary>
    /// 当前场景的游戏状态
    /// </summary>
    public GameSceneState State { get; set; } = GameSceneState.Loading;

    /// <summary>
    /// 场景加载的时间戳
    /// </summary>
    public DateTime LoadTime { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// 场景实例的唯一标识符
    /// </summary>
    public string InstanceId { get; init; } = string.Empty;
}