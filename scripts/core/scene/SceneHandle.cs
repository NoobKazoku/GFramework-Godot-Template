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

namespace GFrameworkGodotTemplate.scripts.core.scene;

/// <summary>
/// 场景句柄结构体，用于安全地引用和标识场景实例。
/// 通过场景键和实例ID的组合确保对特定场景实例的唯一引用。
/// </summary>
/// <param name="key">场景的唯一标识键</param>
/// <param name="instanceId">场景实例的唯一标识符</param>
public readonly struct SceneHandle(string key, string instanceId)
{
    /// <summary>
    /// 获取场景的唯一标识键
    /// </summary>
    public string Key { get; init; } = key;

    /// <summary>
    /// 获取场景实例的唯一标识符
    /// </summary>
    public string InstanceId { get; init; } = instanceId;
}