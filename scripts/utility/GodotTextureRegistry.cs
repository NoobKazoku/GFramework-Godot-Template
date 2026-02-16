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

using GFramework.Core.Abstractions.registries;
using Godot;

namespace GFrameworkGodotTemplate.scripts.utility;

/// <summary>
///     GodotTextureRegistry 类继承自 KeyValueRegistryBase 并实现 IGodotTextureRegistry 接口。
///     该类用于管理纹理资源的注册表，提供基于字符串键的纹理查找功能。
/// </summary>
/// <remarks>
///     此类使用 StringComparer.Ordinal 作为键的比较器，确保键的比较行为一致且高效。
/// </remarks>
public class GodotTextureRegistry()
    : KeyValueRegistryBase<string, Texture>(StringComparer.Ordinal), IGodotTextureRegistry;