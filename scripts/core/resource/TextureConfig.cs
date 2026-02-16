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

using GFramework.Core.Abstractions.bases;
using GFrameworkGodotTemplate.scripts.enums.resources;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.resource;

/// <summary>
///     TextureConfig 类用于配置纹理资源，实现 IKeyValue 接口以提供键值对访问。
///     该类继承自 Godot 的 Resource 类，并通过导出属性支持编辑器配置。
/// </summary>
[GlobalClass]
public partial class TextureConfig : Resource, IKeyValue<string, Texture2D>
{
    /// <summary>
    ///     获取或设置纹理的键值，用于标识纹理资源。
    ///     该属性通过 [Export] 特性暴露给 Godot 编辑器。
    /// </summary>
    [Export]
    public TextureKey TextureKey { get; private set; }

    /// <summary>
    ///     获取或设置实际的纹理资源对象。
    ///     该属性通过 [Export] 特性暴露给 Godot 编辑器，默认值为 null。
    /// </summary>
    [Export]
    public Texture2D Texture { get; private set; } = null!;

    /// <summary>
    ///     实现 IKeyValue 接口的 Key 属性，返回 TextureKey 的字符串表示形式。
    /// </summary>
    public string Key => TextureKey.ToString();

    /// <summary>
    ///     实现 IKeyValue 接口的 Value 属性，返回当前配置的纹理资源。
    /// </summary>
    public Texture2D Value => Texture;
}