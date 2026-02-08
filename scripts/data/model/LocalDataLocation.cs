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

using GFramework.Game.Abstractions.data;
using GFramework.Game.Abstractions.enums;

namespace GFrameworkGodotTemplate.scripts.data.model;

/// <summary>
///     表示本地数据位置的实现类，实现了IDataLocation接口
/// </summary>
public sealed record LocalDataLocation : IDataLocation
{
    /// <summary>
    ///     获取数据位置的键值，固定为"local"
    /// </summary>
    public string Key { get; init; } = "local";

    /// <summary>
    ///     获取存储类型，固定为StorageKind.Local
    /// </summary>
    public StorageKinds Kinds { get; init; } = StorageKinds.Local;

    /// <summary>
    ///     获取命名空间，固定为空字符串
    /// </summary>
    public string? Namespace { get; init; } = "";

    /// <summary>
    ///     获取元数据字典，固定为null
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; } = null;
}